using Lab2.Model;
using System.Collections.Generic;

namespace Lab2.Repositories
{
    public interface ICustomerRepository
    {
        void ReadFromFile();
        void WriteToFile();
        int Add(Customer customer);
        Customer GetCustomer(int id);
        int Remove(int id);
        int Change(int id, Customer newCustomer);
        List<Customer> ListCustomers();
    }
}