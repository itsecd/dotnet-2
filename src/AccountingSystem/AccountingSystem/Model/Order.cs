using System.Collections.Generic;

namespace AccountingSystem.Model
{
    /// <summary>Class to save info about order</summary>
    public class Order
    {
        public virtual int OrderId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual int Status { get; set; }
        public virtual double Price { get; set; }
        public virtual IList<Product> Products { get; set; }

    }
}
