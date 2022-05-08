using System.Collections.Generic;
using Lab2.Model;

namespace Lab2.Repository
{
    public interface ICustomerRepository
    {
        List<Customer> GetAllCustomers();
        void AddCustomer(Customer customer);
        Customer GetCustomer(int id);
        void DeleteAllCustomers();
        int DeleteCustomer(int id);
        int ReplaceCustomer(int id, Customer newCustomer);
        void ReadFromFileCustomers();
        void WriteToFileCustomers();
    }
}