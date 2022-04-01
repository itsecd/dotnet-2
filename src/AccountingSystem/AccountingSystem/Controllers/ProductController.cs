using AccountingSystem.Model;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


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

        [HttpGet("{id:int}")]
        public Product Get(int id)
        {
            return _repository.GetProduct(id);
        }

        [HttpPost]
        public int Post([FromBody] Product product)
        {
            return _repository.AddProduct(product);
        }

        [HttpPut("{id:int}")]
        public int Put(int id, [FromBody] Product product)
        {
            return _repository.ChangeProduct(id, product);
        }

        [HttpDelete("{id:int}")]
        public int Delete(int id)
        {
            return _repository.RemoveProduct(id);
        }
    }
}
