using LibApp.Models;
using LibApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LibApp.Data;

namespace LibApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private string _hostUrl;

        public CustomersController(HttpClient httpClient, IHttpContextAccessor contextAccessor, ApplicationDbContext context)
        {
            _context = context;
            _httpClient = httpClient;
            _hostUrl = $"{contextAccessor.HttpContext.Request.Scheme}://{contextAccessor.HttpContext.Request.Host}";
        }

        public ViewResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            using var client = new HttpClient();

            var result = await client.GetAsync($"{_hostUrl}/api/customers/{id}");
            
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();

            if ( content == null)
            {
                return Content("User not found");
            }

            var customer = JsonConvert.DeserializeObject<Customer>(content);

            return View(customer);
        }

        public IActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();

            var viewModel = new CustomerFormViewModel()
            {
                MembershipTypes = membershipTypes
            };

            return View("CustomerForm", viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            using var client = new HttpClient();

            var result = await client.GetAsync($"{_hostUrl}/api/customers/{id}");

            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();

            if (content == null)
            {
                return Content("User not found");
            }

            var customer = JsonConvert.DeserializeObject<Customer>(content);

            if (customer == null)
            {
                return NotFound();
            }

            var viewModel = new CustomerFormViewModel(customer)
            {
                MembershipTypes = _context.MembershipTypes.ToList()
            };

            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel(customer)
                {
                    MembershipTypes = _context.MembershipTypes.ToList()
                };

                return View("CustomerForm", viewModel);
            }
            if (customer.Id == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);
                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.HasNewsletterSubscribed = customer.HasNewsletterSubscribed;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Index", "Customers");
        }
    }
}