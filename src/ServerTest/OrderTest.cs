using Lab2.Model;
using Lab2.Repository;
using System;
using System.Collections.Generic;
using Xunit;

namespace ServerTest
{
    public class OrderTest
    {
        private static Order CreateOrder(int orderId, int customerId, DateTime date)
        {
            var testCustomer = new Customer
            {
                Id = customerId,
                Name = "Doraemon",
                PhoneNumber = "123456"
            };
            var listProducts = new List<Product>()
            {
                new Product
                {
                    Id = 11,
                    Name = "Apple",
                    Price = 22
                },
                new Product
                {
                    Id = 99,
                    Name = "Rice",
                    Price = 33
                },
                new Product
                {
                    Id = 999,
                    Name = "Mango",
                    Price = 55
                }
            };
            var testOrder = new Order
            {
                OrderId = orderId,
                CustomerId = customerId,
                Products = listProducts,
                Date = date,
                Status = "done"
            };
            return testOrder;
        }

        [Fact]
        public void AddOrderTest()
        {
            var testOrder = CreateOrder(12, 22, DateTime.Now);
            OrderRepository orderRepository = new OrderRepository();
            orderRepository.Add(testOrder);
            Assert.Equal(testOrder.OrderId, orderRepository.GetOrder(12).OrderId);
            Assert.Equal(testOrder.CustomerId, orderRepository.GetOrder(12).CustomerId);
            orderRepository.Remove(12);
        }

        [Fact]
        public void DeleteOrderTest()
        {
            var testOrder = CreateOrder(12, 22, DateTime.Now);
            OrderRepository orderRepository = new();
            orderRepository.Add(testOrder);
            Assert.Equal(testOrder.OrderId, orderRepository.Remove(12));
        }

        [Fact]
        public void ChangeOrder()
        {
            var testOrder = CreateOrder(12, 22, DateTime.Now);
            OrderRepository orderRepository = new();
            orderRepository.Add(testOrder);
            //var newOrder = CreateOrder(15, 45, DateTime.Now);
            Assert.Equal(testOrder.OrderId, orderRepository.Change(testOrder.OrderId, testOrder));
            orderRepository.Remove(testOrder.OrderId);
        }

        [Fact]
        public void AddProducttest()
        {
            var testOrder = CreateOrder(12, 22, DateTime.Now);
            var newProduct = new Product
            {
                Id = 11,
                Name = "Star Fruit",
                Price = 14
            };
            OrderRepository orderRepository = new();
            Assert.Equal(testOrder.OrderId, orderRepository.AddProduct(testOrder.OrderId, newProduct));
            Assert.Equal(testOrder.SumOrder + newProduct.Price, orderRepository.GetOrder(testOrder.OrderId).SumOrder);
            orderRepository.Remove(testOrder.OrderId);
        }

        [Fact]
        public void RemoveProductTest()
        {
            var testOrder = CreateOrder(12, 22, DateTime.Now);
            var product = new Product
            {
                Id = 99,
                Name = "Rice",
                Price = 33
            };
            OrderRepository orderRepository = new();
            orderRepository.Add(testOrder);
            Assert.Equal(product.Id, orderRepository.RemoveProduct(testOrder.OrderId, product.Id));
            Assert.Equal(testOrder.SumOrder - product.Price, orderRepository.GetOrder(testOrder.OrderId).SumOrder);
            orderRepository.Remove(testOrder.OrderId);
        }

        [Fact]
        public void ChangeProductTest()
        {
            var testOrder = CreateOrder(12, 22, DateTime.Now);
            var newProduct = new Product
            {
                Id = 26,
                Name = "Dragon Fruit",
                Price = 30
            };
            OrderRepository orderRepository = new();
            orderRepository.Add(testOrder);
            orderRepository.AddProduct(testOrder.OrderId, newProduct);
            Assert.Equal(newProduct.Id, orderRepository.ChangeProduct(testOrder.OrderId, newProduct.Id, newProduct));
            orderRepository.Remove(testOrder.OrderId);
        }

        [Fact]
        public void BestProductOfMont()
        {
            var testOrder = CreateOrder(12, 22, DateTime.Now);
            var product = new Product
            {
                Id = 111,
                Name = "Coconut",
                Price = 555
            };
            OrderRepository orderRepository = new();
            orderRepository.Add(testOrder);
            orderRepository.AddProduct(testOrder.OrderId, product);
            Assert.Equal(product.Price, orderRepository.BestProductOfMonth().Value);
            orderRepository.Remove(testOrder.OrderId);
        }
    }
}
