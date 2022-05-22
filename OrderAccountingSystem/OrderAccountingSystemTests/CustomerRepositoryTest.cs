using Microsoft.Extensions.Configuration;
using OrderAccountingSystem.Models;
using OrderAccountingSystem.Repository;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace OrderAccountingSystemTests
{
    public class CustomerRepositoryTest
    {
        private static readonly IConfigurationRoot _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        [Fact]
        public async Task AddCustomerTestAsync()
        {
            CustomerRepository repository = new CustomerRepository(_config);
            Customer customer = GenerateCustomer();
            Guid customerId = await repository.AddCustomerAsync(customer);
            Assert.True(await repository.CheckCustomerAsync(customerId));
            await repository.DeleteCustomerAsync(customerId);
        }

        [Fact]
        public async Task DeleteCustomerTestAsync()
        {
            CustomerRepository repository = new CustomerRepository(_config);
            Customer customer = GenerateCustomer();
            Guid customerId = await repository.AddCustomerAsync(customer);
            Assert.True(await repository.CheckCustomerAsync(customerId));
            Assert.Equal(await repository.DeleteCustomerAsync(customerId), customerId);
            Assert.False(await repository.CheckCustomerAsync(customerId));
        }

        [Fact]
        public async Task ChangeCustomerTestAsync()
        {
            CustomerRepository repository = new CustomerRepository(_config);
            Customer customer = GenerateCustomer();
            Guid customerId = await repository.AddCustomerAsync(customer);
            Assert.True(await repository.CheckCustomerAsync(customerId));
            Assert.Equal(await repository.ChangeCustomerAsync(customerId, GenerateCustomer()), customerId);
            await repository.DeleteCustomerAsync(customerId);
        }

        private Customer GenerateCustomer()
        {
            Customer customer = new Customer("Anton", "88887776655");
            return customer;
        }
    }
}
