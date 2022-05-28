using System;

namespace OrderAccountingSystemClient.Models
{
    /// <summary>Info about customer</summary>
    [System.Serializable]
    public class Customer
    {
        /// <summary>Customer ID</summary>
        public string CustomerId { get; set; } = string.Empty;

        /// <summary>Customer Name</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Customer Phone Number</summary>
        public string Phone { get; set; } = string.Empty;

    }
}
