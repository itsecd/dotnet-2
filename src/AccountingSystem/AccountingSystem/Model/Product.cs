using System;

namespace AccountingSystem.Model
{
    ///<summary>Class to save info about product</summary>
    public class Product
    {
        public virtual int ProductId { get; set; }
        public virtual string Name { get; set; }
        public virtual double Price { get; set; }
        public virtual DateTime Date { get; set; }

    }
}
