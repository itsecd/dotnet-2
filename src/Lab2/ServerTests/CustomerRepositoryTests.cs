using Lab2.Model;
using Lab2.Repository;
using Xunit;

namespace ServerTests
{
    public class CustomerRepositoryTests
    {
        private static Customer CreateCustomer(string phone)
        {
            var customer = new Customer
            {
                FullName = "Popov Ivan",
                PhoneNumber = phone

            };
            return customer;
        }
        [Fact]
        public void AddCustomerTest()
        {
            var customer = CreateCustomer("89274563212");
            CustomerRepository customerRepository = new();
            customerRepository.AddCustomer(customer);
            Assert.Equal(customer.PhoneNumber, customerRepository.GetCustomer(1).PhoneNumber);
            customerRepository.DeleteCustomer(customer.Id);
        }

        [Fact]
        public void DeleteCustomerTest()
        {
            var customer = CreateCustomer("89192563475");
            CustomerRepository customerRepository = new();
            customerRepository.AddCustomer(customer);
            Assert.Equal(customer.Id, customerRepository.DeleteCustomer(1));

        }

        [Fact]
        public void ReplaceCustomerTest()
        {
            var customer = CreateCustomer("85697453214");
            CustomerRepository customerRepository = new();
            customerRepository.AddCustomer(customer);
            Assert.Equal(customer.Id, customerRepository.ReplaceCustomer(1, customer));
            customerRepository.DeleteCustomer(customer.Id);
        }
    }
}
