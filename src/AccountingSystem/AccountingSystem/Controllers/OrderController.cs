using AccountingSystem.Exeption;
using AccountingSystem.Model;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AccountingSystem.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;

        public OrderController(IOrderRepository repository)
        {
            _repository = repository;
        }

        /// <summary>Get All Order</summary>
        /// <returns>All Order</returns>
        [HttpGet]
        public ActionResult<IList<Order>> Get()
        {
            try
            {
                return _repository.GetOrders().ToList();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>Get Order By ID</summary>
        /// <returns>Order</returns>
        [HttpGet("{id:int}")]
        public ActionResult<Order> Get(int id)
        {
            try
            {
                return _repository.GetOrder(id);
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>Get All Price By Order</summary>
        /// <returns>All Price By Order</returns>
        [HttpGet("all-price")]
        public ActionResult<double> GetAllPrice()
        {
            try
            {
                return _repository.GetAllPrice();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>Get All Count Products By Order Monthly</summary>
        /// <returns>All Count Products</returns>
        [HttpGet("products-monthly")]
        public ActionResult<int> GetCountProductMonthly()
        {
            try
            {
                return _repository.GetCountProductMonthly();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>Add Order To DataBase</summary>
        /// <returns>Order ID</returns>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Order order)
        {
            try
            {
                return _repository.AddOrder(order);
            }
            catch
            {
                return Problem();
            }

        }

        /// <summary>Change Order In DataBase</summary>
        /// <returns>Order ID</returns>
        [HttpPut("{id:int}")]
        public ActionResult<int> Put(int id, [FromBody] Order order)
        {
            try
            {
                return _repository.ChangeOrder(id, order);
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>Change Order Status</summary>
        /// <returns>Order ID</returns>
        [HttpPatch("{id:int}")]
        public ActionResult<int> PatchStatus(int id, [FromBody] Order order)
        {
            try
            {
                return _repository.PatchStatus(id, order);
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch
            {
                return Problem();
            }

        }

        /// <summary>Delete Order From DataBase</summary>
        /// <returns>Order ID</returns>
        [HttpDelete("{id:int}")]
        public ActionResult<int> Delete(int id)
        {
            try
            {
                return _repository.RemoveOrder(id);
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>Get All Product From Order</summary>
        /// <returns>All Products</returns>
        [HttpGet("{id:int}/products")]
        public ActionResult<IList<Product>> GetProducts(int id)
        {
            try
            {
                return _repository.GetProducts(id).ToList();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>Get Product By ID From Order</summary>
        /// <returns>Product</returns>
        [HttpGet("{id:int}/products/{productId:int}")]
        public ActionResult<Product> GetProduct(int id, int productId)
        {
            try
            {
                return _repository.GetProduct(id, productId);
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>Add Product To Order</summary>
        /// <returns>Product ID</returns>
        [HttpPost("{id:int}/products")]
        public ActionResult<int> PostProduct([FromBody] Product product, int id)
        {
            try
            {
                return _repository.AddProduct(product, id);
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>Change Product In Order</summary>
        /// <returns>Product ID</returns>
        [HttpPut("{id:int}/products/{productId:int}")]
        public ActionResult<int> PutProduct(int id, [FromBody] Product product, int productId)
        {
            try
            {
                return _repository.ChangeProduct(id, product, productId);
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>Change Order Status</summary>
        /// <returns>Order ID</returns>
        [HttpPatch("{id:int}/products/{productId:int}")]
        public ActionResult<int> PatchProduct(int id, [FromBody] Product product, int productId)
        {
            try
            {
                return _repository.ChangeProduct(id, product, productId);
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch
            {
                return Problem();
            }

        }

        /// <summary>Delete Product From Order</summary>
        /// <returns>Product ID</returns>
        [HttpDelete("{id:int}/products/{productId:int}")]
        public ActionResult<int> DeleteProduct(int id, int productId)
        {
            try
            {
                return _repository.RemoveProduct(id, productId);
            }
            catch (NotFoundInDatabaseException)
            {
                return NotFound();
            }
            catch
            {
                return Problem();
            }
        }


    }
}
