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

        [HttpGet]
        public IList<Order> Get()
        {
            return _repository.GetOrders();
        }

        [HttpGet("{id}")]
        public Order Get(int id)
        {
            return _repository.GetOrder(id);
        }

        [HttpPost]
        public void Post([FromBody] Order order)
        {
            _repository.AddOrder(order);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Order order)
        {
            _repository.ChangeOrder(id, order);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repository.RemoveOrder(id);
        }
    }
}
