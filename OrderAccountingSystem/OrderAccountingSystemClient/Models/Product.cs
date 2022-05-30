namespace OrderAccountingSystemClient.Models
{
    ///<summary>Info about product</summary>
    [System.Serializable]
    public class Product
    {
        /// <summary>Product ID</summary>
        public string ProductId { get; set; } = string.Empty;
        /// <summary>Product Name</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Product Price</summary>
        public double Price { get; set; }

        public Product()
        {
        }

        public Product(string productId, string name, double price)
        {
            ProductId = productId;
            Name = name;
            Price = price;
        }
    }
}
