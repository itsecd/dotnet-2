using AccountingSystem.Model;
using AccountingSystem.Repository;
using System;
using Xunit;

namespace TestServerAccountingSystem
{
    public class CustomerRepositoryFixture : IDisposable
    {
        public CustomerRepositoryFixture()
        {
            var customer = new Customer
            {
                CustomerId = 57,
                Name = "Vova",
                Phone = "888",
                Address = "SPB"
            };
            CustomerRepository repository = new();
            Assert.Equal(57, repository.AddCustomer(customer));
        }

        public void Dispose()
        {
            CustomerRepository repository = new();
            Assert.Equal(57, repository.RemoveCustomer(57));
        }
    }

    public class CustomerRepositoryTests : IClassFixture<CustomerRepositoryFixture>
    {

        [Fact]
        public void ChangeCustomer()
        {
            var customer = new Customer
            {
                CustomerId = 57,
                Name = "VovaDD",
                Phone = "88822",
                Address = "MSK"
            };
            CustomerRepository repository = new();
            Assert.Equal(57, repository.ChangeCustomer(57, customer));
        }

    }
}


