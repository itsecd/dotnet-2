using System;
using System.Collections.Generic;
using Lab2.Exeptions;
using Lab2.Model;
using Lab2.Repository;
using Microsoft.AspNetCore.Mvc;

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
        // GET: api/<OrderController>
        [HttpGet]
        public IEnumerable<Order> GetAll()
        {
            return _repository.GetAllOrders();
        }
        // GET api/<OrderController>/
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
        // POST api/<OrderController>
        [HttpPost]
        public void Post([FromBody] Order order)
        {
            _repository.AddOrder(order);
        }
        // PUT api/<OrderController>/5
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
        // DELETE api/<OrderController>/5
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
        [HttpDelete]
        public void DeleteAll()
        {
            _repository.DeleteAllOrders();

        }
    }
}
