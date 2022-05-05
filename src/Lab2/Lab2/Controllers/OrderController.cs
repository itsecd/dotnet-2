using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2.Model;
using Lab2.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public IEnumerable<Order> Get()
        {
            return _repository.GetAllOrders();
        }
        // GET api/<OrderController>/
        [HttpGet("{id}")]
        public Order Get(int id)
        {
            return _repository.GetOrder(id);
        }
        // POST api/<OrderController>
        [HttpPost]
        public void Post([FromBody] Order order)
        {
            _repository.AddOrder(order);
        }
        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Order order)
        {
            _repository.ReplaceOrder(id, order);
        }
        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repository.DeleteOrder(id);
        }
        [HttpDelete]
        public void DeleteAll()
        {
            _repository.DeleteAllOrders();

        }
    }
}
