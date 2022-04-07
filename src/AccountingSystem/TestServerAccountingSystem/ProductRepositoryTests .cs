using AccountingSystem.Model;
using AccountingSystem.Repository;
using System;
using Xunit;

namespace TestServerAccountingSystem
{
    public class ProductRepositoryTests 
    {


        [Fact]
        public void AddProduct()
        {
            Product product = new Product
            {
                ProductId = 57,
                Name = "IPhone",
                Price = 7000,
                Date = System.DateTime.Now,

            };
            ProductRepository repository = new();
            int count = repository.GetProducts().Count;
            Assert.Equal(57, repository.AddProduct(product));
            Assert.Equal(count + 1, repository.GetProducts().Count);
            repository.RemoveProduct(57);
        }

        [Fact]
        public void ChangeProduct()
        {
            Product product = new Product
            {
                ProductId = 36,
                Name = "IPhone",
                Price = 7000,
                Date = System.DateTime.Now,

            };
            ProductRepository repository = new();
            repository.AddProduct(product);
            Assert.Equal(36, repository.ChangeProduct(36, product));
            repository.RemoveProduct(36);
        }

        [Fact]
        public void RemoveProduct()
        {
            Product product = new Product
            {
                ProductId = 64,
                Name = "IPhone",
                Price = 7000,
                Date = System.DateTime.Now,

            };
            ProductRepository repository = new();
            repository.AddProduct(product);
            int count = repository.GetProducts().Count;
            Assert.Equal(64, repository.RemoveProduct(64));
            Assert.Equal(count - 1, repository.GetProducts().Count);

        }

    }
}
