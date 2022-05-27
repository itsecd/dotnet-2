using System;

namespace OrderAccountingSystemClient.Models
{
    /// <summary>Info about customer</summary>
    [System.Serializable]
    public class Customer
    {
        /// <summary>Customer ID</summary>
        public Guid CustomerId { get; set; }

        /// <summary>Customer Name</summary>
        public string Name { get; set; }

        /// <summary>Customer Phone Number</summary>
        public string Phone { get; set; }

        public Customer()
        {
        }

        public Customer(string name, string phone)
        {
            CustomerId = Guid.NewGuid();
            Name = name;
            Phone = phone;
        }

        public Customer(Guid customerId, string name, string phone)
        {
            CustomerId = customerId;
            Name = name;
            Phone = phone;
        }

    }
}
