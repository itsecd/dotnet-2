using OrderAccountingSystem.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderAccountingSystem.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Guid> AddProductAsync(Product product);
        Task<Product> GetProductAsync(Guid id);
        Task<Guid> DeleteProductAsync(Guid id);
        Task<Guid> ChangeProductAsync(Guid id, Product newProduct);
        Task<bool> CheckProductAsync(Guid id);
        Task ReadProductsFileAsync();
        Task WriteProductsFileAsync();
    }
}
