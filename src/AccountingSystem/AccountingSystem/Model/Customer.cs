using System;
using System.Net.Http;

namespace AccountingSystem.Model
{
    /// <summary>Class to save info about customer</summary>
    public class Customer
    {
        /// <summary>Field Including Customer ID</summary>
        public virtual int CustomerId { get; set; }
        /// <summary>Field Including Customer Name</summary>
        public virtual string Name { get; set; }
        /// <summary>Field Including Customer Phone Number</summary>
        public virtual string Phone { get; set; }
        /// <summary>Field Including Customer Address</summary>
        public virtual string Address { get; set; }

        public static implicit operator Customer(HttpResponseMessage v)
        {
            throw new NotImplementedException();
        }
    }
}
