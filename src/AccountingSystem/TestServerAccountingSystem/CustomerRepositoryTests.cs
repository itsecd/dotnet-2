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
            Customer customer = new Customer();
            customer.CustomerId = 57;
            customer.Name = "Vova";
            customer.Phone = "888";
            customer.Address = "SPB";
            CustomerRepository repository = new();
            Assert.Equal(1, repository.AddCustomer(customer));
        }

        [Fact]
        public void ChangeCustomer()
        {
            Customer customer = new Customer();
            customer.CustomerId = 57;
            customer.Name = "VovaDD";
            customer.Phone = "88822";
            customer.Address = "MSK";
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
