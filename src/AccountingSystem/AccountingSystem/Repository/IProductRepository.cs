using AccountingSystem.Model;
using System.Collections.Generic;

namespace AccountingSystem.Repository
{
    public interface IProductRepository
    {
        IList<Product> GetProducts();
        Product GetProduct(int id);
        int ChangeProduct(int id, Product product);
        int AddProduct(Product product);
        int RemoveProduct(int id);
    }
}
