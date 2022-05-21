using System;

namespace OrderAccountingSystem.Models
{
    ///<summary>Info about product</summary>
    [System.Serializable]
    public class Product
    {
        /// <summary>Product ID</summary>
        public Guid ProductId { get; set; }
        /// <summary>Product Name</summary>
        public string Name { get; set; }

        /// <summary>Product Price</summary>
        public double Price { get; set; }

        public Product()
        {
        }

        public Product(string name, double price)
        {
            ProductId = Guid.NewGuid();
            Name = name;
            Price = price;
        }

        public Product(Guid productId, string name, double price)
        {
            ProductId = productId;
            Name = name;
            Price = price;
        }
    }
}
