using Lab2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Lab2.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private const string StorageFileName = "DataCustomers.xml";
        private List<Customer>? _customers;

        public void ReadFromFile()
        {
            if (_customers != null) return;
            if (!File.Exists(StorageFileName))
            {
                _customers = new List<Customer>();
                return;
            }
            var xmlSerializer = new XmlSerializer(typeof(List<Customer>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Open);
            _customers = (List<Customer>)(xmlSerializer.Deserialize(fileStream) ?? throw new InvalidOperationException());
        }

        public void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Customer>));
            using (var fileStream = new FileStream(StorageFileName, FileMode.Create))
            {
                xmlSerializer.Serialize(fileStream, _customers);
            }
        }

        public int Add(Customer customer)
        {
            ReadFromFile();
            if (customer == null) throw new ArgumentNullException();
            if (customer.Id < 0) throw new ArgumentException("ID invalid!");
            if (_customers!.Any(cus => cus.Id == customer.Id)) throw new ArgumentException("ID exist!");
            _customers!.Add(customer);
            WriteToFile();
            return customer.Id;
        }

        public int Change(int id, Customer newCustomer)
        {
            ReadFromFile();
            if (id < 0) throw new ArgumentOutOfRangeException();
            if (newCustomer == null) throw new ArgumentNullException();
            if (_customers!.Any(cus => cus.Id == id) == false) throw new NotFoundException();
            var customer = _customers!.Where(cus => cus.Id == id).Single();
            customer.Id = newCustomer.Id;
            customer.Name = newCustomer.Name;
            //customer.Number = newCustomer.Number;
            WriteToFile();
            return newCustomer.Id;
        }

        public List<Customer> ListCustomers()
        {
            ReadFromFile();
            return _customers!;
        }

        public Customer GetCustomer(int id)
        {
            ReadFromFile();
            if (id < 0) throw new ArgumentOutOfRangeException();
            var customer = _customers!.FirstOrDefault(cus => cus.Id == id);
            if (customer is null) throw new NotFoundException();
            return customer;
        }

        public int Remove(int id)
        {
            ReadFromFile();
            if (id < 0) throw new ArgumentOutOfRangeException();
            var customer = GetCustomer(id);
            if (customer is null) throw new NotFoundException();
            _customers!.Remove(customer);
            WriteToFile();
            return id;
        }
    }
}
