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
            Product product = new Product();
            product.ProductId = 57;
            product.Name = "Motorolla";
            product.Price = 3000;
            product.Date = System.DateTime.Now;
            ProductRepository repository = new();
            Assert.Equal(1, repository.AddProduct(product));
        }

        [Fact]
        public void ChangeProduct()
        {
            Product product = new Product();
            product.ProductId = 57;
            product.Name = "Motorolla";
            product.Price = 3000;
            product.Date = System.DateTime.Now;
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
