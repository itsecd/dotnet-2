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

        /// <summary>Get All Product</summary>
        [HttpGet]
        public IList<Product> Get()
        {
            return _repository.GetProducts();
        }

        /// <summary>Get Product By ID</summary>
        [HttpGet("{id:int}")]
        public ActionResult<Product> Get(int id)
        {
            return _repository.GetProduct(id);
        }

        /// <summary>Add Product To DataBase</summary>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Product product)
        {
            try
            {
                return _repository.AddProduct(product);
            }
            catch
            {
                return Conflict();
            }
        }

        /// <summary>Change Product In DataBase</summary>
        [HttpPut("{id:int}")]
        public ActionResult<int> Put(int id, [FromBody] Product product)
        {
            try
            {
                return _repository.ChangeProduct(id, product);
            }
            catch
            {
                return Conflict();
            }
        }

        /// <summary>Delete Product From DataBase</summary>
        [HttpDelete("{id:int}")]
        public ActionResult<int> Delete(int id)
        {
            try
            {
                return _repository.RemoveProduct(id);
            }        
            catch
            {
                return Conflict();
            }
        }
    }
}
