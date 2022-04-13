﻿using AccountingSystem.Connection;
using AccountingSystem.Model;
using AccountingSystem.Repository;
using Xunit;

namespace TestServerAccountingSystem
{
    public class CustomerRepositoryFixture
    {
        public CustomerRepositoryFixture()
        {
            NHibernateSession.GenerateSchema();
        }
    }

    public class CustomerRepositoryTests : IClassFixture<CustomerRepositoryFixture>
    {
    
        [Fact]
        public void AddCustomer()
        {
            var customer = new Customer
            {
                CustomerId = 57,
                Name = "VovaDD",
                Phone = "88822",
                Address = "MSK"
            };
            CustomerRepository repository = new();
            int count = repository.GetCustomers().Count;
            Assert.Equal(57, repository.AddCustomer(customer));
            Assert.Equal(count + 1, repository.GetCustomers().Count);
            repository.RemoveCustomer(57);
        }


        [Fact]
        public void ChangeCustomer()
        {
            var customer = new Customer
            {
                CustomerId = 36,
                Name = "VovaDD",
                Phone = "88822",
                Address = "MSK"
            };
            CustomerRepository repository = new();
            repository.AddCustomer(customer);
            Assert.Equal(36, repository.ChangeCustomer(36, customer));
            repository.RemoveCustomer(36);
        }

        [Fact]
        public void RemoveCustomer()
        {
            var customer = new Customer
            {
                CustomerId = 64,
                Name = "VovaDD",
                Phone = "88822",
                Address = "MSK"
            };
            CustomerRepository repository = new();
            repository.AddCustomer(customer);
            int count = repository.GetCustomers().Count;
            Assert.Equal(64, repository.RemoveCustomer(64));
            Assert.Equal(count - 1, repository.GetCustomers().Count);

        }

    }
}


