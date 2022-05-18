using OrderAccountingSystem.Model;
using OrderAccountingSystem.Repositories;
using System;
using Xunit;

namespace OrderAccountingSystemTests
{
    public class ProductRepositoryTest
    {

        [Fact]
        public void AddProductTest()
        {
            ProductRepository repository = new ProductRepository();
            Product product = GenerateProduct();
            Guid productId = repository.AddProduct(product);
            Assert.True(repository.CheckProduct(productId));
            repository.DeleteProduct(productId);
        }

        [Fact]
        public void DeleteProductTest()
        {
            ProductRepository repository = new ProductRepository();
            Product product = GenerateProduct();
            Guid productId = repository.AddProduct(product);
            Assert.True(repository.CheckProduct(productId));
            Assert.Equal(repository.DeleteProduct(productId), productId);
            Assert.False(repository.CheckProduct(productId));
        }

        private Product GenerateProduct()
        {
            Product product = new Product("Ananas", 123.4);
            return product;
        }
    }
}
