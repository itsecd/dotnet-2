using OrderAccountingSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderAccountingSystem.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Guid> AddProductAsync(Product product);
        Task<Product> GetProductAsync(Guid id);
        Task<Guid> DeleteProductAsync(Guid id);
        Task<Guid> ChangeProductAsync(Guid id, Product newProduct);
        Task<bool> CheckProductAsync(Guid id);
    }
}
