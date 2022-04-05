using System.Collections.Generic;

namespace AccountingSystem.Model
{
    /// <summary>Class to save info about order</summary>
    public class Order
    {
        /// <summary>Field Including Order ID</summary>
        public virtual int OrderId { get; set; }
        /// <summary>Field Including Order Customer</summary>
        public virtual Customer Customer { get; set; }
        /// <summary>Field Including Order Status</summary>
        public virtual int Status { get; set; }
        /// <summary>Field Including Order Price</summary>
        public virtual double Price { get; set; }
        /// <summary>Field Including Products In Order</summary>
        public virtual IList<Product> Products { get; set; }

    }
}
