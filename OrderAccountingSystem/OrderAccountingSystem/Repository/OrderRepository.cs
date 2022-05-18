using OrderAccountingSystem.Exceptions;
using OrderAccountingSystem.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace OrderAccountingSystem.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private List<Product> _products;
        private readonly string _fileName = "Products.xml";

        public OrderRepository()
        {
            _products = new List<Product>();
        }
        public List<Product> GetAllProducts()
        {
            ReadProductsFile();
            return _products;
        }

        public Product GetProduct(Guid id)
        {
            ReadProductsFile();
            foreach (Product product in _products)
            {
                if (product.ProductId.Equals(id))
                {
                    return product;
                }
            }
            throw new NotFoundException();
        }

        public Guid AddProduct(Product product)
        {
            ReadProductsFile();
            _products.Add(product);
            WriteProductsFile();
            return product.ProductId;
        }

        public Guid ChangeProduct(Guid id, Product newProduct)
        {
            ReadProductsFile();
            foreach(Product product in _products)
            {
                if(product.ProductId == id)
                {
                    product.Price = newProduct.Price;
                    product.Name = newProduct.Name;
                }
            }
            WriteProductsFile();
            return id;
        }

        public bool CheckProduct(Guid id)
        {
            ReadProductsFile();
            if(_products.Find(f => f.ProductId.Equals(id)) != null){
                return true;
            }
            return false;
        }

        public Guid DeleteProduct(Guid id)
        {
            ReadProductsFile();
            _products.Remove(GetProduct(id));
            WriteProductsFile();
            return id;
        }

        public void ReadProductsFile()
        {
            if (!File.Exists(_fileName))
            {
                _products = new List<Product>();
                return;
            }
            XmlSerializer formatter = new XmlSerializer(typeof(List<Product>));
            FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate);
            _products = (List<Product>)formatter.Deserialize(fs);
            fs.Close();
        }

        public void WriteProductsFile()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Product>));
            FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate);
            formatter.Serialize(fs, _products);
            fs.Close();
        }

    }
}
