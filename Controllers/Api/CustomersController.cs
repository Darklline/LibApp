using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LibApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        public CustomersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET /api/customers
        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _context.Customers
                .Include(c => c.MembershipType)
                .ToList()
                .Select(_mapper.Map<Customer, CustomerDto>);

            return Ok(customers);
        }


        // GET /api/customers/{id}
        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            Console.WriteLine("Request beginning");
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);
            await Task.Delay(2000);
            if (customer == null)
            {
                return NotFound();
            }

            Console.WriteLine("Request end");
            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        [HttpGet("details/{id}")]
        public IActionResult GetCustomerDetails(int id)
        {
            return Redirect("https://localhost:5001/customers/details/" + id);
        }

        // POST /api/customers
        [HttpPost]
        public IActionResult CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var customer = _mapper.Map<Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();
            customerDto.Id = customer.Id;

            return CreatedAtRoute(nameof(GetCustomer), new { id = customerDto.Id }, customerDto);
        }

        // PUT /api/customers
        [HttpPut("{id}")]
        [Authorize(Roles = "Owner")]
        public void UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            }

            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customerInDb == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            _mapper.Map(customerDto, customerInDb);
            _context.SaveChanges();
        }

        // DELETE /api/customers
        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
        public void DeleteCustomer(int id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customerInDb == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            _context.Remove(customerInDb);
            _context.SaveChanges();
        }

        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
    }
}
