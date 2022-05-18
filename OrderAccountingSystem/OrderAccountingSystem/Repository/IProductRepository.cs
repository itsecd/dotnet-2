using OrderAccountingSystem.Model;
using System;
using System.Collections.Generic;

namespace OrderAccountingSystem.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        Guid AddProduct(Product product);
        Product GetProduct(Guid id);
        Guid DeleteProduct(Guid id);
        Guid ChangeProduct(Guid id, Product newProduct);
        bool CheckProduct(Guid id);
        void ReadProductsFile();
        void WriteProductsFile();
    }
}
