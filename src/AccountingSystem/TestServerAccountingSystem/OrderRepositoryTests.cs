using AccountingSystem.Model;
using AccountingSystem.Repository;
using System;
using System.Collections.Generic;
using Xunit;

namespace TestServerAccountingSystem
{
    public class OrderRepositoryFixture : IDisposable
    {
        public OrderRepositoryFixture()
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
                Date = DateTime.Now,
            };

            Order order = new Order
            {
                OrderId = 123,
                Customer = customer,
                Status = 0,
                Price = 0,
                Date = DateTime.Now,
                Products = new List<Product>() { product }
            };

            OrderRepository repository = new();
            Assert.Equal(123, repository.AddOrder(order));
        }

        public void Dispose()
        {
            OrderRepository repository = new();
            Assert.Equal(123, repository.RemoveOrder(123));
        }
    }

    public class OrderRepositoryTests : IClassFixture<OrderRepositoryFixture>
    {

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
                Date = DateTime.Now,
            };

            Order order = new Order
            {
                OrderId = 123,
                Customer = customer,
                Status = 7,
                Price = 0,
                Date = DateTime.Now,
                Products = new List<Product>() { product }
            };

            OrderRepository repository = new();
            Assert.Equal(123, repository.ChangeOrder(123, order));
        }



        [Fact]
        public void PatchStatus()
        {
            OrderRepository repository = new();
            Order order = new Order
            {
                Status = 7
            };
            Assert.Equal(123, repository.PatchStatus(123, order));
        }

    }
}
