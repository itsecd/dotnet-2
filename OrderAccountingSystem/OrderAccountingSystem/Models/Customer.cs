namespace OrderAccountingSystem.Model
{
    [System.Serializable]
    /// <summary>Info about customer</summary>
    public class Customer
    {
        /// <summary>Customer ID</summary>
        public int CustomerId { get; set; }

        /// <summary>Customer Name</summary>
        public string FullName { get; set; }

        /// <summary>Customer Phone Number</summary>
        public string Phone { get; set; }

    }
}
