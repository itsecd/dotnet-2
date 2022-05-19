using OrderAccountingSystem.Exceptions;
using OrderAccountingSystem.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OrderAccountingSystem.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private List<Customer> _customers;
        private static SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly string _fileName = "Customers.xml";

        public CustomerRepository()
        {
            _customers = new List<Customer>();
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            await ReadCustomersFileAsync();
            return _customers;
        }

        public async Task<Guid> AddCustomerAsync(Customer customer)
        {
            if (customer.Name == "" || customer.Phone == "")
            {
                throw new ArgumentException();
            }
            await ReadCustomersFileAsync();
            _customers.Add(customer);
            await WriteCustomersFileAsync();
            return customer.CustomerId;
        }

        public async Task<Customer> GetCustomerAsync(Guid id)
        {
            await ReadCustomersFileAsync();
            foreach (Customer customer in _customers)
            {
                if (customer.CustomerId.Equals(id))
                {
                    return customer;
                }
            }
            throw new NotFoundException();
        }

        public async Task<Guid> DeleteCustomerAsync(Guid id)
        {
            await ReadCustomersFileAsync();
            if (_customers.Remove(_customers.Find(f => f.CustomerId == id)))
            {
                await WriteCustomersFileAsync();
                return id;
            }
            throw new NotFoundException();
        }

        public async Task<Guid> ChangeCustomerAsync(Guid id, Customer newCustomer)
        {
            if (newCustomer.Name == "")
            {
                throw new ArgumentException();
            }
            await ReadCustomersFileAsync();
            foreach (Customer customer in _customers)
            {
                if (customer.CustomerId == id)
                {
                    customer.Phone = newCustomer.Phone;
                    customer.Name = newCustomer.Name;
                    await WriteCustomersFileAsync();
                    return id;
                }
            }
            throw new NotFoundException();
        }

        public async Task<bool> CheckCustomerAsync(Guid id)
        {
            await ReadCustomersFileAsync();
            if (_customers.Find(f => f.CustomerId.Equals(id)) != null)
            {
                return true;
            }
            return false;
        }

        public async Task ReadCustomersFileAsync()
        {
            await SemaphoreSlim.WaitAsync();
            try
            {
                if (!File.Exists(_fileName))
                {
                    _customers = new List<Customer>();
                    return;
                }
                XmlSerializer formatter = new XmlSerializer(typeof(List<Customer>));
                FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate);
                _customers = (List<Customer>)formatter.Deserialize(fs);
                fs.Close();
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        public async Task WriteCustomersFileAsync()
        {
            await SemaphoreSlim.WaitAsync();
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(List<Customer>));
                FileStream fs = new FileStream(_fileName, FileMode.Create);
                formatter.Serialize(fs, _customers);
                fs.Close();
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
    }
}
