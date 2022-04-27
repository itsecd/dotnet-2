using System.Collections.Generic;
using System.Threading.Tasks;
using Lab2.Model;

namespace Lab2.Repository
{
    public interface ICustomerRepository
    {
        List<Customer> GetAllCustomers();
        public void AddCustomer(Customer customer);
        public Customer GetCustomer(int id);
        public void DeleteAllCustomers();
        public void DeleteCustomer(int id);
        public void ReplaceCustomer(Customer customer, int id);



    }
}