using AccountingSystem.Model;
using AccountingSystem.Repository;
using System;
using Xunit;

namespace TestServerAccountingSystem
{
    public class ProductRepositoryFixture : IDisposable
    {
        public ProductRepositoryFixture()
        {
            Product product = new Product
            {
                ProductId = 57,
                Name = "Motorolla",
                Price = 3000,
                Date = System.DateTime.Now,

            };
            ProductRepository repository = new();
            Assert.Equal(57, repository.AddProduct(product));
        }

        public void Dispose()
        {
            ProductRepository repository = new();
            Assert.Equal(57, repository.RemoveProduct(57));
        }
    }

    public class ProductRepositoryTests : IClassFixture<ProductRepositoryFixture>
    {

        [Fact]
        public void ChangeProduct()
        {
            Product product = new Product
            {
                ProductId = 57,
                Name = "IPhone",
                Price = 7000,
                Date = System.DateTime.Now,

            };
            ProductRepository repository = new();
            Assert.Equal(57, repository.ChangeProduct(57, product));
        }

    }
}
