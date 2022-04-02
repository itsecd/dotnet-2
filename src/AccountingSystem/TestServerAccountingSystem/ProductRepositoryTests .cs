using AccountingSystem.Model;
using AccountingSystem.Repository;
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
                Name = "Motorolla",
                Price = 3000,
                Date = System.DateTime.Now,

            };
            ProductRepository repository = new();
            Assert.Equal(1, repository.AddProduct(product));
        }

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
            Assert.Equal(1, repository.ChangeProduct(57, product));
        }

        [Fact]
        public void RemoveProduct()
        {
            ProductRepository repository = new();
            Assert.Equal(1, repository.RemoveProduct(57));
        }

    }
}
