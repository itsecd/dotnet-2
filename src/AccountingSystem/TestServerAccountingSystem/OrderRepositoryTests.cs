using AccountingSystem.Model;
using AccountingSystem.Repository;
using System.Collections.Generic;
using Xunit;

namespace TestServerAccountingSystem
{
    public class OrderRepositoryTests
    {

        [Fact]
        public void AddOrder()
        {
            var customer = new Customer
            {
                CustomerId = 57,
                Name = "Vova",
                Phone = "888",
                Address = "SPB"
            };

            Product product = new Product
            {
                ProductId = 57,
                Name = "Motorolla",
                Price = 3000,
                Date = System.DateTime.Now,
            };

            Order order = new Order
            {
                OrderId = 123,
                Customer = customer,
                Status = 0,
                Price = 0,
                Products = new List<Product>() { product }
            };

            OrderRepository repository = new();
            Assert.Equal(123, repository.AddOrder(order));
        }

        [Fact]
        public void ChangeOrder()
        {
            var customer = new Customer
            {
                CustomerId = 57,
                Name = "Vova",
                Phone = "888",
                Address = "SPB"
            };

            Product product = new Product
            {
                ProductId = 57,
                Name = "Motorolla",
                Price = 3000,
                Date = System.DateTime.Now,

            };

            Order order = new Order
            {
                OrderId = 123,
                Customer = customer,
                Status = 7,
                Price = 0,
                Products = new List<Product>() { product }
            };

            OrderRepository repository = new();
            Assert.Equal(123, repository.ChangeOrder(123, order));
        }

        [Fact]
        public void PatchStatus()
        {
            OrderRepository repository = new();
            Assert.Equal(123, repository.PatchStatus(123, 123));
        }

        [Fact]
        public void RemoveOrder()
        {
            OrderRepository repository = new();
            Assert.Equal(123, repository.RemoveOrder(123));
        }

    }
}
