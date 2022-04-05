namespace AccountingSystem.Model
{
    /// <summary>Class to save info about customer</summary>
    public class Customer
    {
        public virtual int CustomerId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Address { get; set; }

    }
}
