using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Lab2.Model;
using Microsoft.Extensions.Configuration;

namespace Lab2.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _storageFileName;
        public CustomerRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("CustomersFile");
        }
        private List<Customer> _customers { get; set; }
        public void ReadFromFileCustomers()
        {
            if (!File.Exists(_storageFileName))
            {
                _customers = new List<Customer>();
                return;
            }
            using (var fileReader = new StreamReader(_storageFileName))
            {
                string jsonString = fileReader.ReadToEnd();
                _customers = JsonSerializer.Deserialize<List<Customer>>(jsonString);
            }
            
        }
        public void WriteToFileCustomers()
        {
            string jsonString = JsonSerializer.Serialize(_customers);
            using (var fileWriter = new StreamWriter(_storageFileName))
            {
                fileWriter.Write(jsonString);
            }
        }
        public void AddCustomer(Customer customer)
        {
            _customers.Add(customer);           
        }
        public void ReplaceCustomer(int id, Customer customer)
        {
            var customerIndex = _customers.FindIndex(customer => customer.Id == id);
            if (customerIndex > 0)
            {
                _customers[customerIndex] = customer;
            }
        }
        public List<Customer> GetAllCustomers()
        {
            return _customers;
        }
        public Customer GetCustomer(int id)
        {
            Customer customer =  _customers.Where(customer => customer.Id == id).Single();
            return customer;
        }
        public void DeleteCustomer(int id)
        {
            _customers.Remove(_customers.Single(customer => customer.Id == id));

        }
        public void DeleteAllCustomers()
        {
            _customers.Clear();
        }
    }
}
