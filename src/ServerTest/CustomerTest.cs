using Lab2.Model;
using Lab2.Repository;
using System;
using System.Collections.Generic;
using Xunit;

namespace ServerTest
{
    public class CustomerTest
    {
        private static Customer Creat(int id, string name)
        {
            var customer = new Customer
            {
                Id = id,
                Name = name,
                Number = new PhoneNumber
                {
                    CountryCode = new List<byte> ( 7 ),
                    LocalCode = new List<byte> { 9,6,7},
                    LastNumber = new List<byte> { 7,2,4,4,5,0,1}
                }
            };
            return customer;
        }

        [Fact]
        public void Add()
        {
            var testCustomer = Creat(12, "Doraemon");
            CustomerRepository customerRepository = new();

            customerRepository.Add(testCustomer);

            Assert.Equal(testCustomer.Id, customerRepository.GetCustomer(12).Id);
            customerRepository.Remove(testCustomer.Id);
        }

        [Fact]
        public void Delete()
        {
            var testCustomer = Creat(12, "Doraemon");
            CustomerRepository customerRepository = new();
            customerRepository.Add(testCustomer);

            var IDRemove = customerRepository.Remove(testCustomer.Id);

            Assert.Equal(testCustomer.Id, IDRemove);
        }

        [Fact]
        public void Change()
        {
            var testCustomer = Creat(12, "Doraemon");
            CustomerRepository customerRepository = new();
            customerRepository.Add(testCustomer);
            var newCustomer = Creat(13, "Nobita");

            var IDTest = customerRepository.Change(12, newCustomer);

            Assert.Equal(newCustomer.Id, IDTest);
            customerRepository.Remove(newCustomer.Id);
        }
    }
}
