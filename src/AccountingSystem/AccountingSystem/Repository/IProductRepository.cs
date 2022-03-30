using AccountingSystem.Model;
using System.Collections.Generic;

namespace AccountingSystem.Repository
{
    public interface IProductRepository
    {
        IList<Product> GetProducts();
        Product GetProduct(int id);
        void ChangeProduct(int id, Product product);
        void AddProduct(Product product);
        void RemoveProduct(int id);
    }
}
