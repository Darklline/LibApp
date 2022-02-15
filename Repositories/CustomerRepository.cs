using LibApp.Data;
using LibApp.Models;
using System.Linq;

namespace LibApp.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Customer GetCustomer(int id)
        {
            return _context.Customers.Single(c => c.Id == id);
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
