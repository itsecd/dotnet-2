namespace OrderAccountingSystemClient.Models
{
    /// <summary>Info about order</summary>
    [System.Serializable]
    public class Order
    {
        /// <summary>Order ID</summary>
        public string OrderId { get; set; } = string.Empty;

        /// <summary>Order Customer</summary>
        public string Customer { get; set; } = string.Empty;

        /// <summary>Products In Order</summary>
        public string Products { get; set; } = string.Empty;

        /// <summary>Order Status</summary>
        public int Status { get; set; }

        /// <summary>Order Price</summary>
        public double Price { get; set; }

        /// <summary>Order Date</summary>
        public string Date { get; set; } = string.Empty;

    }
}
