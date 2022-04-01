using AccountingSystem.Model;
using System.Collections.Generic;

namespace AccountingSystem.Repository
{
    public interface ICustomerRepository
    {
        IList<Customer> GetCustomers();
        Customer GetCustomer(int id);
        int ChangeCustomer(int id, Customer customer);
        int AddCustomer(Customer customer);
        int RemoveCustomer(int id);
    }
}
