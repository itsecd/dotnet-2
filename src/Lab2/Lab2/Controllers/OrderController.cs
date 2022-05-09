using System;
using System.Collections.Generic;
using Lab2.Exeptions;
using Lab2.Model;
using Lab2.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers
{
    /// <summary>
    /// Сlass responsible for processing incoming requests and performing operations on orders
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        public OrderController(IOrderRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Getting all orders
        /// </summary>
        /// <returns>All orders</returns>
        [HttpGet]
        public IEnumerable<Order> GetAll()
        {
            return _repository.GetAllOrders();
        }
        /// <summary>
        /// Getting order by ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Order</returns>
        [HttpGet("{id}")]
        public ActionResult<Order> Get(int id)
        {
            try
            {
                return _repository.GetOrder(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }
            
        }
        /// <summary>
        /// Adding order
        /// </summary>
        /// <param name="order">Added order</param>
        /// <returns>Order ID</returns>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Order order)
        {
            try
            {
                return _repository.AddOrder(order);
            }
            catch(ArgumentException)
            {
                return BadRequest();
            }
            
        }
        /// <summary>
        /// Changing the order by ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="order">Changeable order</param>
        /// <returns>Order ID</returns>
        [HttpPut("{id}")]
        public ActionResult<int> Put(int id, [FromBody] Order order)
        {
            try
            {
               return _repository.ReplaceOrder(id, order);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }

        }
        /// <summary>
        /// Deleting order by ID
        /// </summary>
        /// <param name="id">ID of the order being deleted</param>
        /// <returns>Order ID</returns>
        [HttpDelete("{id}")]
        public ActionResult<int> Delete(int id)
        {
            try
            {
                return _repository.DeleteOrder(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }
        }
        /// <summary>
        /// Deleting all orders
        /// </summary>
        [HttpDelete]
        public void DeleteAll()
        {
            _repository.DeleteAllOrders();

        }
        /// <summary>
        /// Getting all products of the order by Order ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>All products of the order</returns>
        [HttpGet("{id}/Products")]
        public ActionResult<List<Product>> GetProducts(int id)
        {
            try
            {
                return _repository.GetProducts(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }
        }
        /// <summary>
        /// Adding product to an order by Order ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="product">Added product</param>
        /// <returns>Order ID</returns>
        [HttpPost("{id}/Products")]
        public ActionResult<int> PostProduct(int id, Product product)
        {
            try
            {
                return _repository.AddProduct(id, product);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }

        }
        /// <summary>
        /// Getting an order product by its number in the list
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="num">Number of the product</param>
        /// <returns>Product</returns>
        [HttpGet("{id}/Products/{num}")]
        public ActionResult<Product> GetProduct(int id, int num)
        {
            try
            {
                return _repository.GetProduct(id, num);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }
        }
        /// <summary>
        /// Deleting an order product by its number in the list
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="num">Number of the product in list</param>
        /// <returns>Order ID </returns>
        [HttpDelete("{id}/Products/{num}")]
        public ActionResult<int> DeleteProduct(int id, int num)
        {
            try
            {
                return _repository.RemoveProduct(id, num);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }

        }
        /// <summary>
        /// Changing the order product
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="num">Number of the product</param>
        /// <param name="product">New product</param>
        /// <returns>Order ID</returns>
        [HttpPut("{id}/Products/{num}")]
        public ActionResult<int> PutProduct(int id, int num, Product product)
        {
            try
            {
                return _repository.ReplaceProduct(id, num, product);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }

        }
        /// <summary>
        /// Deleting all products from the order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Order ID</returns>
        [HttpDelete("{id}/Products")]
        public ActionResult<int> DeleteAllProducts(int id)
        {
            try
            {
                return _repository.RemoveAllProducts(id);
            }
            
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }

        }
        /// <summary>
        /// Getting the name and amount of products per month
        /// </summary>
        /// <returns>Pair: name and amount of products ordered</returns>
        [HttpGet("products per month")]
        public ActionResult<Dictionary<string, int>> GetProductMonth()
        {
            try
            {
                return _repository.GetProductsMonth();
            }
            catch
            {
                return Problem();
            }
        }
        /// <summary>
        /// Getting the total cost of orders for the month
        /// </summary>
        /// <returns>Total cost</returns>
        [HttpGet("total cost")]
        public ActionResult<float> GetTotalCostMonth()
        {
            try
            {
                return _repository.GetAmountMonth();
            }
            catch
            {
                return Problem();
            }
        }

    }
}
