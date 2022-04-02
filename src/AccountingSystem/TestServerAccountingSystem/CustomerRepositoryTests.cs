using AccountingSystem.Model;
using AccountingSystem.Repository;
using Xunit;

namespace TestServerAccountingSystem
{
    public class CustomerRepositoryTests
    {

        [Fact]
        public void AddCustomer()
        {
            var customer = new Customer
            {
                CustomerId = 57,
                Name = "Vova",
                Phone = "888",
                Address = "SPB"
            };
            CustomerRepository repository = new();
            Assert.Equal(1, repository.AddCustomer(customer));
        }

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
            Assert.Equal(1, repository.ChangeCustomer(57, customer));
        }

        [Fact]
        public void RemoveCustomer()
        {
            CustomerRepository repository = new();
            Assert.Equal(1, repository.RemoveCustomer(57));
        }

    }
}
