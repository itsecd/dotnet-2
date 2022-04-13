using AccountingSystem.Connection;
using AccountingSystem.Model;
using AccountingSystem.Repository;
using System;
using System.Collections.Generic;
using Xunit;

namespace TestServerAccountingSystem
{
    public class OrderRepositoryTests : IClassFixture<RepositoryFixture>
    {
        [Fact]
        public void AddOrder()
        {
            OrderRepository repository = new();
            int count = repository.GetOrders().Count;
            Assert.Equal(57, repository.AddOrder(CreateOrder(57)));
            Assert.Equal(count + 1, repository.GetOrders().Count);
            repository.RemoveOrder(57);
        }

        [Fact]
        public void ChangeOrder()
        {
            OrderRepository repository = new();
            repository.AddOrder(CreateOrder(36));
            Assert.Equal(36, repository.ChangeOrder(36, CreateOrder(36)));
            repository.RemoveOrder(36);
        }

        [Fact]
        public void PatchStatus()
        {
            OrderRepository repository = new();
            repository.AddOrder(CreateOrder(39));
            Order order = new Order
            {
                Status = 7
            };
            Assert.Equal(39, repository.PatchStatus(39, order));
            repository.RemoveOrder(39);
        }

        [Fact]
        public void RemoveOrder()
        {
            OrderRepository repository = new();
            repository.AddOrder(CreateOrder(64));
            int count = repository.GetOrders().Count;
            Assert.Equal(64, repository.RemoveOrder(64));
            Assert.Equal(count - 1, repository.GetOrders().Count);
        }

        [Fact]
        public void AddProduct()
        {
            Product product = new Product
            {
                ProductId = 57,
                Name = "IPhone",
                Price = 7000,
                Date = System.DateTime.Now,

            };
            OrderRepository repository = new();
            repository.AddOrder(CreateOrder(67));
            int count = repository.GetProducts(67).Count;
            Assert.Equal(67, repository.AddProduct(product, 67));
            Assert.Equal(count + 1, repository.GetProducts(67).Count);
            repository.RemoveOrder(67);
        }

        [Fact]
        public void ChangeProduct()
        {
            Product product = new Product
            {
                ProductId = 36,
                Name = "IPhone",
                Price = 7000,
                Date = System.DateTime.Now,

            };
            OrderRepository repository = new();
            repository.AddOrder(CreateOrder(75));
            repository.AddProduct(product, 75);
            Assert.Equal(36, repository.ChangeProduct(75, product, 36));
            repository.RemoveOrder(75);
        }

        [Fact]
        public void RemoveProduct()
        {
            Product product = new Product
            {
                ProductId = 64,
                Name = "IPhone",
                Price = 7000,
                Date = System.DateTime.Now,

            };
            OrderRepository repository = new();
            repository.AddOrder(CreateOrder(84));
            repository.AddProduct(product, 84);
            int count = repository.GetProducts(84).Count;
            Assert.Equal(84, repository.RemoveProduct(84, 64));
            Assert.Equal(count - 1, repository.GetProducts(84).Count);

        }

        private Order CreateOrder(int id)
        {
            var rand = new Random();
            var customer = new Customer
            {
                CustomerId = rand.Next(),
                Name = "Vova",
                Phone = "888",
                Address = "SPB"
            };

            Product product = new Product
            {
                ProductId = rand.Next(),
                Name = "Motorolla",
                Price = 3000,
                Date = DateTime.Now,
            };

            Order order = new Order
            {
                OrderId = id,
                Customer = customer,
                Status = 7,
                Price = 0,
                Date = DateTime.Now,
                Products = new List<Product>() { product }
            };
            return order;
        }

    }
}
