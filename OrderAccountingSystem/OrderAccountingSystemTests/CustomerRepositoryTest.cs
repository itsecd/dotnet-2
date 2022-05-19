using OrderAccountingSystem.Model;
using OrderAccountingSystem.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OrderAccountingSystemTests
{
    public class CustomerRepositoryTest
    {

        [Fact]
        public async Task AddCustomerTestAsync()
        {
            CustomerRepository repository = new CustomerRepository();
            Customer customer = GenerateCustomer();
            Guid customerId = await repository.AddCustomerAsync(customer);
            Assert.True(await repository.CheckCustomerAsync(customerId));
            await repository.DeleteCustomerAsync(customerId);
        }

        [Fact]
        public async Task DeleteCustomerTestAsync()
        {
            CustomerRepository repository = new CustomerRepository();
            Customer customer = GenerateCustomer();
            Guid customerId = await repository.AddCustomerAsync(customer);
            Assert.True(await repository.CheckCustomerAsync(customerId));
            Assert.Equal(await repository.DeleteCustomerAsync(customerId), customerId);
            Assert.False(await repository.CheckCustomerAsync(customerId));
        }

        [Fact]
        public async Task ChangeCustomerTestAsync()
        {
            CustomerRepository repository = new CustomerRepository();
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
