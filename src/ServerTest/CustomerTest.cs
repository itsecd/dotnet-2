using Lab2.Model;
using Lab2.Repository;
using Xunit;

namespace ServerTest
{
    public class CustomerTest
    {
        private static Customer CreatCustomer(int id, string name, string phone)
        {
            var customer = new Customer
            {
                Id = id,
                Name = name,
                PhoneNumber = phone
            };
            return customer;
        }

        [Fact]
        public void Add()
        {
            var testCustomer = CreatCustomer(12, "Doraemon", "123456");
            CustomerRepository customerRepository = new();
            customerRepository.Add(testCustomer);
            Assert.Equal(testCustomer.Name, customerRepository.GetCustomer(12).Name);
            Assert.Equal(testCustomer.PhoneNumber, customerRepository.GetCustomer(12).PhoneNumber);
            customerRepository.Remove(testCustomer.Id);
        }

        [Fact]
        public void Delete()
        {
            var testCustomer = CreatCustomer(12, "Doraemon", "123456");
            CustomerRepository customerRepository = new();
            customerRepository.Add(testCustomer);
            Assert.Equal(testCustomer.Id, customerRepository.Remove(12));
        }

        [Fact]
        public void Change()
        {
            var testCustomer = CreatCustomer(12, "Doraemon", "123456");
            CustomerRepository customerRepository = new();
            customerRepository.Add(testCustomer);
            var newCustomer = CreatCustomer(13, "Nobita", "456789");
            Assert.Equal(newCustomer.Id, customerRepository.Change(12, newCustomer));
            customerRepository.Remove(newCustomer.Id);
        }
    }
}
