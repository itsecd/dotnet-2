using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Lab2.Exeptions;
using Lab2.Model;
using Microsoft.Extensions.Configuration;

namespace Lab2.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private List<Customer> _customers;
        private readonly string _storageFileName;
        public CustomerRepository()
        {
            _customers = new();
        }
        public CustomerRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("CustomersFile");
        }
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
        public int AddCustomer(Customer customer)
        {

            if (_customers.Count == 0)
            {
                customer.Id = 1;
            }
            else
            {
                customer.Id = _customers.Max(custom => custom.Id) + 1;
            }
            if (_customers.Any(cstmr => cstmr.FullName == customer.FullName && cstmr.PhoneNumber == customer.PhoneNumber))
            {
                throw new ArgumentException();
            }
            _customers.Add(customer);
            return customer.Id;
        }
        public int ReplaceCustomer(int id, Customer newCustomer)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            var customerIndex = _customers.FindIndex(customer => customer.Id == id);
            
            if (customerIndex == -1)
            {
                throw new NotFoundException();
            }
            Customer customer = _customers[customerIndex];
            customer.FullName = newCustomer.FullName;
            customer.PhoneNumber = newCustomer.PhoneNumber;
            return id;
        }
        public List<Customer> GetAllCustomers()
        {
            return _customers;
        }
        public Customer GetCustomer(int id)
        {
            if( id < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            var customer = _customers.FirstOrDefault(cstmr => cstmr.Id == id);
            if (customer is null)
            {
                throw new NotFoundException();
            }
            return customer;
        }
        public int DeleteCustomer(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            Customer customer = GetCustomer(id);
            if(customer == null)
            {
                throw new NotFoundException();
            }
            _customers.Remove(customer);
            return id;
        }
        public void DeleteAllCustomers()
        {
            _customers.Clear();
        }
    }
}
