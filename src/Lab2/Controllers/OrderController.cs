using Lab2.Model;
using Lab2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Lab2.Controllers
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

        /// <summary>
        /// Получение списки заказов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Order> GetListOrders() => _repository.ListOrders();

        /// <summary>
        /// Получение заказа по идентификатору заказа
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Order> Get(int id)
        {

            try
            {
                return _repository.GetOrder(id);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("ID invalid!");
            }
            catch (NotFoundException)
            {
                return NotFound("ID does not exist!");
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Добавление заказа
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Order order)
        {
            try
            {
                return _repository.Add(order);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Data is empty!");
            }
            catch (ArgumentException e)
            {
                if (e.Message == "ID invalid!") return BadRequest("Order ID invalid!");
                if (e.Message == "ID exist!") return Problem("Order ID existed!");
                return Problem();
            }
        }

        /// <summary>
        /// Редактирование заказ по идентификатору заказа
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newOrder"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<int> Put(int id, [FromBody] Order newOrder)
        {
            try
            {
                return _repository.Change(id, newOrder);
            }
            catch (NotFoundException)
            {
                return NotFound("ID does not exist!");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("ID invalid!");
            }
            catch (ArgumentNullException)
            {
                return Problem("Data is empty!");
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Удаление заказа
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult<int> Delete(int id)
        {
            try
            {
                return _repository.Remove(id);
            }
            catch (NotFoundException)
            {
                return NotFound("Id does not exist!");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("ID invalid!");
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Получение списки продуктов в заказе
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/Products")]
        public ActionResult<List<Product>> GetListProducts(int id)
        {
            try
            {
                return _repository.ListProducts(id);
            }
            catch (NotFoundException)
            {
                return NotFound("ID does not exist!");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("ID invalid!");
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Добавление продукта в заказ
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="newProduct"></param>
        /// <returns></returns>
        [HttpPost("{orderId}/Product")]
        public ActionResult<int> PostProduct(int orderId, Product newProduct)
        {
            try
            {
                return _repository.AddProduct(orderId, newProduct);
            }
            catch (NotFoundException)
            {
                return NotFound("ID does not exist!");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("ID invalid!");
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Data of product is empty!");
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Получение продукта в заказе по индентификатору
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("{orderId}/Product/{productId}")]
        public ActionResult<Product> GetProduct(int orderId, int productId)
        {
            try
            {
                return _repository.GetProduct(orderId, productId);
            }
            catch (NotFoundException e)
            {
                if (e.Message == "Order ID does not exist!") return NotFound("Order ID does not exist!");
                if (e.Message == "Product ID does not exist!") return NotFound("Product ID does not exist!");
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("ID invalid!");
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Удаление продукта из заказа
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("{orderId}/Product/{productId}")]
        public ActionResult<int> DeleteProduct(int orderId, int productId)
        {
            try
            {
                return _repository.RemoveProduct(orderId, productId);
            }
            catch (NotFoundException e)
            {
                if (e.Message == "Order ID does not exist!") return NotFound("Order ID does not exist!");
                if (e.Message == "Product ID does not exist!") return NotFound("Product ID does not exist!");
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("ID invalid!");
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Редактирование продукта в заказе по идентифиуктор
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <param name="newProduct"></param>
        /// <returns></returns>
        [HttpPut("{orderId}/Product/{productId}")]
        public ActionResult<int> PutProduct(int orderId, int productId, Product newProduct)
        {
            try
            {
                return _repository.ChangeProduct(orderId, productId, newProduct);
            }
            catch (NotFoundException e)
            {
                if (e.Message == "Order ID does not exist!") return NotFound("Order ID does not exist!");
                if (e.Message == "Product ID does not exist!") return NotFound("Product ID does not exist!");
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("ID invalid!");
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Получение самого продающегося продукта за месяц и его суммарной стоимости
        /// </summary>
        /// <returns></returns>
        [HttpGet("Best seller in month")]
        public ActionResult<KeyValuePair<Product, float>> GetBestProductOfMonth()
        {
            try
            {
                return _repository.BestProductOfMonth();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Получение суммы за месяц
        /// </summary>
        /// <returns></returns>
        [HttpGet("Total in month")]
        public ActionResult<float> GetSumMonth()
        {
            try
            {
                return _repository.GetSumMonth();
            }
            catch
            {
                return Problem();
            }
        }

    }
}
