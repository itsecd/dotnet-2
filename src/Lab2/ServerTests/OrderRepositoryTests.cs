using System;
using System.Collections.Generic;
using Lab2.Model;
using Lab2.Repository;
using Xunit;

namespace ServerTests
{
    public class OrderRepositoryTests
    {
        private Order CreateOrder(DateTime Data)
        {
            var customer = new Customer
            {
                Id = 1,
                FullName = "Karenina Anna",
                PhoneNumber = "89274563210"
            };
            var products = new List<Product>()
            {
                new Product
                {
                    NameProduct = "apple",
                    CostProduct = 60
                },
                new Product
                {
                    NameProduct = "mango",
                    CostProduct = 70
                }

            };
            var order = new Order
            {
                Products = products,
                CustomerId = customer.Id,
                AmountOrder = 130,
                Dt = Data,
                Status = "done"
            };
            
            return order;
        }
        [Fact]
        public void AddOrderTest()
        {
            DateTime Data = DateTime.Now;
            var order = CreateOrder(Data);
            OrderRepository orderRepository = new();
            orderRepository.AddOrder(order);
            Assert.Equal(Data, orderRepository.GetOrder(1).Dt);
            orderRepository.DeleteOrder(order.OrderId);
        }
        [Fact]
        public void DeleteOrderTest()
        {
            DateTime Data = DateTime.Now;
            var order = CreateOrder(Data);
            OrderRepository orderRepository = new();
            orderRepository.AddOrder(order);
            Assert.Equal(order.OrderId, orderRepository.DeleteOrder(1));
        }

        [Fact]
        public void ReplaceOrderTest()
        {
            DateTime Data = DateTime.Now;
            var order = CreateOrder(Data);
            OrderRepository orderRepository = new();
            orderRepository.AddOrder(order);
            Assert.Equal(order.OrderId, orderRepository.ReplaceOrder(1,order));
            orderRepository.DeleteOrder(order.OrderId);
        }
        [Fact]
        public void AddProductTest()
        {
            var product = new Product
            {
                NameProduct = "lemon",
                CostProduct = 40
            };
            OrderRepository orderRepository = new();
            DateTime Data = DateTime.Now;
            var order = CreateOrder(Data);
            orderRepository.AddOrder(order);
            Assert.Equal(1, orderRepository.AddProduct(order.OrderId, product));
            orderRepository.DeleteOrder(order.OrderId);
        }
        [Fact]
        public void DeleteProductTest()
        {
            var product = new Product
            {
                NameProduct = "banana",
                CostProduct = 60
            };
            OrderRepository orderRepository = new();
            DateTime Data = DateTime.Now;
            var order = CreateOrder(Data);
            orderRepository.AddOrder(order);
            orderRepository.AddProduct(order.OrderId, product);
            Assert.Equal(1, orderRepository.RemoveProduct(order.OrderId,1));
            orderRepository.DeleteOrder(order.OrderId);
        }
        [Fact]
        public void ReplaceProductTest()
        {
            var product = new Product
            {
                NameProduct = "mango",
                CostProduct = 110
            };
            OrderRepository orderRepository = new();
            DateTime Data = DateTime.Now;
            var order = CreateOrder(Data);
            orderRepository.AddOrder(order);
            orderRepository.AddProduct(order.OrderId, product);
            Assert.Equal(1, orderRepository.ReplaceProduct(order.OrderId, 1, product));
            orderRepository.DeleteOrder(order.OrderId);
        }
    }
}

