using Microsoft.Extensions.Configuration;
using OrderAccountingSystem.Models;
using OrderAccountingSystem.Repository;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace OrderAccountingSystemTests
{
    public class ProductRepositoryTest
    {

        private static readonly IConfigurationRoot _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        [Fact]
        public async Task AddProductTestAsync()
        {
            ProductRepository repository = new ProductRepository(_config);
            Product product = GenerateProduct();
            Guid productId = await repository.AddProductAsync(product);
            Assert.True(await repository.CheckProductAsync(productId));
            await repository.DeleteProductAsync(productId);
        }

        [Fact]
        public async Task DeleteProductTestAsync()
        {
            ProductRepository repository = new ProductRepository(_config);
            Product product = GenerateProduct();
            Guid productId = await repository.AddProductAsync(product);
            Assert.True(await repository.CheckProductAsync(productId));
            Assert.Equal(await repository.DeleteProductAsync(productId), productId);
            Assert.False(await repository.CheckProductAsync(productId));
        }

        [Fact]
        public async Task ChangeProductTestAsync()
        {
            ProductRepository repository = new ProductRepository(_config);
            Product product = GenerateProduct();
            Guid productId = await repository.AddProductAsync(product);
            Assert.True(await repository.CheckProductAsync(productId));
            Assert.Equal(await repository.ChangeProductAsync(productId, GenerateProduct()), productId);
            await repository.DeleteProductAsync(productId);
        }

        private Product GenerateProduct()
        {
            Product product = new Product("Ananas", 123.4);
            return product;
        }
    }
}
