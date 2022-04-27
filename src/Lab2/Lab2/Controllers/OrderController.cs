using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2.Model;
using Lab2.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IOrderRepository repository, ILogger<OrderController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        // GET: api/<OrderController>
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            _logger.LogInformation("Method get for orders is invoked!");
            return _repository.GetAllOrders();
        }

        // GET api/<OrderController>/5
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
        public void Put([FromBody] Order order, int id)
        {
            _repository.ReplaceOrder(order, id);
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
