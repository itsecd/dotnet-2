using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderAccountingSystem.Models
{
    /// <summary>Info about order</summary>
    [System.Serializable]
    public class Order
    {
        /// <summary>Order ID</summary>
        public Guid OrderId { get; set; }

        /// <summary>Order Customer</summary>
        public Customer Customer { get; set; }

        /// <summary>Products In Order</summary>
        public List<Product> Products { get; set; }

        /// <summary>Order Status</summary>
        public int Status { get; set; }

        /// <summary>Order Price</summary>
        public double Price { get; set; }

        /// <summary>Order Date</summary>
        public string Date { get; set; }

        public Order()
        {
            OrderId = Guid.NewGuid();
        }

        public Order(Customer customer, List<Product> products, int status, string date)
        {
            OrderId = Guid.NewGuid();
            Customer = customer;
            Products = products;
            Status = status;
            Date = date;
            Price = products.Sum(f => f.Price);
        }
        public Order(Guid orderId, Customer customer, List<Product> products, int status, string date)
        {
            OrderId = orderId;
            Customer = customer;
            Products = products;
            Status = status;
            Date = date;
            Price = products.Sum(f => f.Price);
        }

    }
}
