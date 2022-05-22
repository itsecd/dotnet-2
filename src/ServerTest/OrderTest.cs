using Lab2.Model;
using Lab2.Repository;
using System;
using System.Collections.Generic;
using Xunit;

namespace ServerTest
{
    public class OrderTest
    {
        private static Order Create(int orderId, int customerId, DateTime date)
        {           
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
                OrderStatus = 0
            };
            return testOrder;
        }

        [Fact]
        public void AddOrderTest()
        {
            var testOrder = Create(12, 22, DateTime.Now);
            OrderRepository orderRepository = new OrderRepository();

            var IDTest = orderRepository.Add(testOrder);

            Assert.Equal(testOrder.OrderId, IDTest);

            orderRepository.Remove(testOrder.OrderId);
        }

        [Fact]
        public void DeleteOrderTest()
        {
            var testOrder = Create(12, 22, DateTime.Now);
            OrderRepository orderRepository = new();
            orderRepository.Add(testOrder);

            var IDTest = orderRepository.Remove(testOrder.OrderId);

            Assert.Equal(testOrder.OrderId, IDTest);
        }

        [Fact]
        public void ChangeOrder()
        {
            var testOrder = Create(12, 22, DateTime.Now);
            OrderRepository orderRepository = new();
            orderRepository.Add(testOrder);

            var IDTest = orderRepository.Change(testOrder.OrderId, testOrder);

            Assert.Equal(testOrder.OrderId, IDTest);
            orderRepository.Remove(testOrder.OrderId);
        }

        [Fact]
        public void AddProducttest()
        {
            var testOrder = Create(12, 22, DateTime.Now);
            var newProduct = new Product
            {
                Id = 11,
                Name = "Star Fruit",
                Price = 14
            };
            OrderRepository orderRepository = new();
            orderRepository.Add(testOrder);
            var IDTest = orderRepository.AddProduct(testOrder.OrderId, newProduct);

            Assert.Equal(testOrder.OrderId, IDTest);
            
            orderRepository.Remove(testOrder.OrderId);
        }

        [Fact]
        public void RemoveProductTest()
        {
            var testOrder = Create(12, 22, DateTime.Now);
            var product = new Product
            {
                Id = 99,
                Name = "Rice",
                Price = 33
            };
            OrderRepository orderRepository = new();
            orderRepository.Add(testOrder);
            orderRepository.AddProduct(testOrder.OrderId, product);

            var IDTest = orderRepository.RemoveProduct(testOrder.OrderId, product.Id);

            Assert.Equal(product.Id, IDTest);

            orderRepository.Remove(testOrder.OrderId);
        }

        [Fact]
        public void ChangeProductTest()
        {
            var testOrder = Create(12, 22, DateTime.Now);
            var newProduct = new Product
            {
                Id = 26,
                Name = "Dragon Fruit",
                Price = 30
            };
            OrderRepository orderRepository = new();
            orderRepository.Add(testOrder);
            orderRepository.AddProduct(testOrder.OrderId, newProduct);

            var IDTest = orderRepository.ChangeProduct(testOrder.OrderId, newProduct.Id, newProduct);

            Assert.Equal(newProduct.Id, IDTest);

            orderRepository.Remove(testOrder.OrderId);
        }

        [Fact]
        public void BestProductOfMont()
        {
            var testOrder = Create(12, 22, DateTime.Now);
            var product = new Product
            {
                Id = 111,
                Name = "Coconut",
                Price = 555
            };
            OrderRepository orderRepository = new();
            orderRepository.Add(testOrder);
            orderRepository.AddProduct(testOrder.OrderId, product);

            var BestPrice = orderRepository.BestProductOfMonth().Value;

            Assert.Equal(product.Price, BestPrice);
            orderRepository.Remove(testOrder.OrderId);
        }
    }
}
