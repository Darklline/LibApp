using LibApp.Builders;
using LibApp.Models;
using LibApp.Repositories;
using LibApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LibApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IApiUrlBuilder _apiUrlBuilder;
        private readonly IMembershipTypesRepository _membershipTypesRepository;
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(HttpClient httpClient, IApiUrlBuilder apiUrlBuilder, IMembershipTypesRepository membershipTypesRepository, ICustomerRepository customerRepository)
        {
            _httpClient = httpClient;
            _apiUrlBuilder = apiUrlBuilder;
            _membershipTypesRepository = membershipTypesRepository;
            _customerRepository = customerRepository;
        }

        public ViewResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var content = await GetCustomerByIdContentAsString(id);

            if (content == null)
            {
                return Content("Customer not found");
            }

            var customer = JsonConvert.DeserializeObject<Customer>(content);

            return View(customer);
        }

        public IActionResult New()
        {
            var membershipTypes = _membershipTypesRepository.GetMembershipTypes();

            var viewModel = new CustomerFormViewModel()
            {
                MembershipTypes = membershipTypes
            };

            return View("CustomerForm", viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var content = await GetCustomerByIdContentAsString(id);

            if (content == null)
            {
                return Content("Customer not found");
            }

            var customer = JsonConvert.DeserializeObject<Customer>(content);

            var viewModel = new CustomerFormViewModel(customer)
            {
                MembershipTypes = _membershipTypesRepository.GetMembershipTypes()
            };

            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel(customer)
                {
                    MembershipTypes = _membershipTypesRepository.GetMembershipTypes()
                };

                return View("CustomerForm", viewModel);
            }
            if (customer.Id == 0)
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                await _httpClient.PostAsync(_apiUrlBuilder.BuildApiUrl("/api/customers"), stringContent);
            }
            else
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                await _httpClient.PutAsync(_apiUrlBuilder.BuildApiUrl($"/api/customers/{customer.Id}"), stringContent);
            }

            return RedirectToAction("Index", "Customers");
        }

        private async Task<string> GetCustomerByIdContentAsString(int id)
        {
            var result = await _httpClient.GetAsync(_apiUrlBuilder.BuildApiUrl($"/api/customers/{id}"));

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return await result.Content.ReadAsStringAsync();
        }
    }
}