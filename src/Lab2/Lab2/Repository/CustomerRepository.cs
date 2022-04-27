using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Lab2.Model;

namespace Lab2.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _storageFileName = "customers.json";

        private List<Customer> _customers { get; set; }

        private void ReadFromFile()
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
       
        private void WriteToFile()
        {
            string jsonString = JsonSerializer.Serialize(_customers);
            using (var fileWriter = new StreamWriter(_storageFileName))
            {
                fileWriter.Write(jsonString);
            }
        }
        public void AddCustomer(Customer customer)
        {
            ReadFromFile();
            _customers.Add(customer);
            WriteToFile();
           
        }
        public void ReplaceCustomer(Customer customer, int id)
        {
            ReadFromFile();
            var customerIndex = _customers.FindIndex(customer => customer.Id == id);
            if (customerIndex > 0)
            {
                _customers[customerIndex] = customer;
            }
            WriteToFile();
        }
        public List<Customer> GetAllCustomers()
        {
            ReadFromFile();
            return _customers;
        }
        public Customer GetCustomer(int id)
        {
            ReadFromFile();
            Customer customer =  _customers.Where(customer => customer.Id == id).Single();
            return customer;
        }
        public void DeleteCustomer(int id)
        {
            ReadFromFile();
            _customers.Remove(_customers.Single(customer => customer.Id == id));
            WriteToFile();

        }
        public void DeleteAllCustomers()
        {
            ReadFromFile();
            _customers.Clear();
            WriteToFile();
        }
    }
}
