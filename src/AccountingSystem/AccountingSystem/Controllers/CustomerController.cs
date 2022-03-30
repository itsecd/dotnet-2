using AccountingSystem.Model;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;


namespace AccountingSystem.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IList<Product> Get()
        {
            return _repository.GetProducts();
        }

        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return _repository.GetProduct(id);
        }

        [HttpPost]
        public void Post([FromBody] Product product)
        {
            _repository.AddProduct(product);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Product product)
        {
            _repository.ChangeProduct(id, product);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repository.RemoveProduct(id);
        }
    }
}
