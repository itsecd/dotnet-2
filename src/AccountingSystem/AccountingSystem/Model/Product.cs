using System;

namespace AccountingSystem.Model
{
    ///<summary>Class to save info about product</summary>
    public class Product
    {
        /// <summary>Field Including Product ID</summary>
        public virtual int ProductId { get; set; }
        /// <summary>Field Including Product Name</summary>
        public virtual string Name { get; set; }
        /// <summary>Field Including Product Price</summary>
        public virtual double Price { get; set; }
        /// <summary>Field Including Product Date Of Delivery</summary>
        public virtual DateTime Date { get; set; }

    }
}
