using Microsoft.Extensions.Configuration;
using OrderAccountingSystem.Models;
using OrderAccountingSystem.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace OrderAccountingSystemTests
{
    public class OrderRepositoryTest
    {

        private static readonly IConfigurationRoot _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        [Fact]
        public async Task AddOrderTestAsync()
        {
            OrderRepository repository = new OrderRepository(_config);
            Order order = GenerateOrder();
            Guid orderId = await repository.AddOrderAsync(order);
            Assert.True(await repository.CheckOrderAsync(orderId));
            await repository.DeleteOrderAsync(orderId);
        }

        [Fact]
        public async Task DeleteOrderTestAsync()
        {
            OrderRepository repository = new OrderRepository(_config);
            Order order = GenerateOrder();
            Guid orderId = await repository.AddOrderAsync(order);
            Assert.True(await repository.CheckOrderAsync(orderId));
            Assert.Equal(await repository.DeleteOrderAsync(orderId), orderId);
            Assert.False(await repository.CheckOrderAsync(orderId));
        }

        [Fact]
        public async Task ChangeOrderTestAsync()
        {
            OrderRepository repository = new OrderRepository(_config);
            Order order = GenerateOrder();
            Guid orderId = await repository.AddOrderAsync(order);
            Assert.True(await repository.CheckOrderAsync(orderId));
            Assert.Equal(await repository.ChangeOrderAsync(orderId, GenerateOrder()), orderId);
            await repository.DeleteOrderAsync(orderId);
        }

        [Fact]
        public async Task ChangeOrderStatusTestAsync()
        {
            OrderRepository repository = new OrderRepository(_config);
            Order order = GenerateOrder();
            Guid orderId = await repository.AddOrderAsync(order);
            Assert.True(await repository.CheckOrderAsync(orderId));
            Assert.Equal(await repository.ChangeOrderAsync(orderId, GenerateOrder()), orderId);
            await repository.DeleteOrderAsync(orderId);
        }

        private Order GenerateOrder()
        {
            Order order = new Order(
                new Customer("Kris", "88888"),
                new List<Product>() { new Product("Banana", 100) },
                1,
                DateTime.Now
                );
            return order;
        }
    }
}
