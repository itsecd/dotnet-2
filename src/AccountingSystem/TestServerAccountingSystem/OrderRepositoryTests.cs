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
            Customer customer = new Customer();
            customer.CustomerId = 57;
            customer.Name = "Vova";
            customer.Phone = "888";
            customer.Address = "SPB";

            Product product = new Product();
            product.ProductId = 57;
            product.Name = "Motorolla";
            product.Price = 3000;
            product.Date = System.DateTime.Now;

            Order order = new Order();
            order.OrderId = 123;
            order.Customer = customer;
            order.Status = 0;
            order.Price = 0;
            order.Products = new List<Product>() { product};

            OrderRepository repository = new();
            Assert.Equal(1, repository.AddOrder(order));
        }

        [Fact]
        public void ChangeOrder()
        {
            Customer customer = new Customer();
            customer.CustomerId = 57;
            customer.Name = "Vova";
            customer.Phone = "888";
            customer.Address = "SPB";

            Product product = new Product();
            product.ProductId = 57;
            product.Name = "Motorolla";
            product.Price = 3000;
            product.Date = System.DateTime.Now;

            Order order = new Order();
            order.OrderId = 123;
            order.Customer = customer;
            order.Status = 0;
            order.Price = 0;
            order.Products = new List<Product>() { product };

            OrderRepository repository = new();
            Assert.Equal(1, repository.ChangeOrder(123, order));
        }

        [Fact]
        public void PatchStatus()
        {
            OrderRepository repository = new();
            Assert.Equal(1, repository.PatchStatus(123, 123));
        }

        [Fact]
        public void RemoveOrder()
        {
            OrderRepository repository = new();
            Assert.Equal(1, repository.RemoveOrder(123));
        }

    }
}
