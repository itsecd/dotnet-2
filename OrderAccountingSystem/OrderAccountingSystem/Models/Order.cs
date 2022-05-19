using System;
using System.Collections.Generic;

namespace OrderAccountingSystem.Model
{
    [System.Serializable]
    /// <summary>Info about order</summary>
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
        public DateTime Date { get; set; }

        public Order()
        {
        }

        public Order(Customer customer, List<Product> products, int status, DateTime date)
        {
            Customer = customer;
            Products = products;
            Status = status;
            Date = date;
        }
        public Order(Guid orderId, Customer customer, List<Product> products, int status, DateTime date)
        {
            OrderId = orderId;
            Customer = customer;
            Products = products;
            Status = status;
            Date = date;
        }

    }
}
