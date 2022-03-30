using System;
using System.Xml.Serialization;

namespace AccountingSystem.Model
{
    public class Product
    {
        public virtual int ProductId { get; set; }
        public virtual string Name { get; set; }
        public virtual double Price { get; set; }
        public virtual DateTime Date { get; set; }

    }
}
