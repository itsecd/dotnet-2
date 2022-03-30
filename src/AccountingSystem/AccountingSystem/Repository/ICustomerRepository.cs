using AccountingSystem.Model;
using System.Collections.Generic;

namespace AccountingSystem.Repository
{
    public interface ICustomerRepository
    {
        IList<Customer> GetCustomers();
        Customer GetCustomer(int id);
        void ChangeCustomer(int  id, Customer customer);
        void AddCustomer(Customer customer);
        void RemoveCustomer(int id);
    }
}
