using System;

namespace OrderAccountingSystem.Model
{
    [System.Serializable]
    ///<summary>Info about product</summary>
    public class Product
    {
        /// <summary>Product ID</summary>
        public int ProductId { get; set; }
        /// <summary>Product Name</summary>
        public string Name { get; set; }

        /// <summary>Product Price</summary>
        public double Price { get; set; }

    }
}
