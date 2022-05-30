using System;
using System.Collections.Generic;

namespace Lab2.Model
{
    /// <summary>
    /// Order
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Order ID
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Summary order
        /// </summary>
        public List<Product> Products { get; set; }
        /// <summary>
        /// Customer ID
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Data order
        /// </summary>
        public DateTime Dt { get; set; }
        /// <summary>
        /// Amount order
        /// </summary>
        public float AmountOrder { get; set; }
        /// <summary>
        /// Status order
        /// </summary>
        public string Status { get; set; }


    }
}
