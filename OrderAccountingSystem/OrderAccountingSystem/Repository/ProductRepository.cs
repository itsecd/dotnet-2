using OrderAccountingSystem.Exceptions;
using OrderAccountingSystem.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OrderAccountingSystem.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products;
        private static SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly string _fileName = "Products.xml";

        public ProductRepository()
        {
            _products = new List<Product>();
        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            await ReadProductsFileAsync();
            return _products;
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            await ReadProductsFileAsync();
            foreach (Product product in _products)
            {
                if (product.ProductId.Equals(id))
                {
                    return product;
                }
            }
            throw new NotFoundException();
        }

        public async Task<Guid> AddProductAsync(Product product)
        {
            if (product.Name == "")
            {
                throw new ArgumentException();
            }
            await ReadProductsFileAsync();
            _products.Add(product);
            await WriteProductsFileAsync();
            return product.ProductId;
        }

        public async Task<Guid> ChangeProductAsync(Guid id, Product newProduct)
        {
            if (newProduct.Name == "")
            {
                throw new ArgumentException();
            }
            await ReadProductsFileAsync();
            foreach(Product product in _products)
            {
                if(product.ProductId == id)
                {
                    product.Price = newProduct.Price;
                    product.Name = newProduct.Name;
                    await WriteProductsFileAsync();
                    return id;
                }
            }
            throw new NotFoundException();
        }

        public async Task<Guid> DeleteProductAsync(Guid id)
        {
            await ReadProductsFileAsync();
            if (_products.Remove(_products.Find(f=> f.ProductId == id)))
            {
                await WriteProductsFileAsync();
                return id;
            }
            throw new NotFoundException();
        }

        public async Task<bool> CheckProductAsync(Guid id)
        {
            await ReadProductsFileAsync();
            if (_products.Find(f => f.ProductId.Equals(id)) != null)
            {
                return true;
            }
            return false;
        }

        public async Task ReadProductsFileAsync()
        {
            await SemaphoreSlim.WaitAsync();
            try
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
            finally
            {
                SemaphoreSlim.Release();
            }

        }

        public async Task WriteProductsFileAsync()
        {
            await SemaphoreSlim.WaitAsync();
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(List<Product>));
                FileStream fs = new FileStream(_fileName, FileMode.Create);
                formatter.Serialize(fs, _products);
                fs.Close();
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

    }
}
