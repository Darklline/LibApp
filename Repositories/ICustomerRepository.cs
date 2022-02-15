using LibApp.Models;

namespace LibApp.Repositories
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customer);
        Customer GetCustomer(int id);
        void SaveChanges();
    }
}