using AccountingSystem.Model;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


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

        [HttpGet("{id:int}")]
        public Order Get(int id)
        {
            return _repository.GetOrder(id);
        }

        [HttpGet("all-price")]
        public double GetAllPrice()
        {
            return _repository.GetAllPrice();
        }

        [HttpPost]
        public int Post([FromBody] Order order)
        {
            return _repository.AddOrder(order);
        }

        [HttpPut("{id:int}")]
        public int Put(int id, [FromBody] Order order)
        {
            return _repository.ChangeOrder(id, order);
        }

        [HttpPatch("status-{id:int}")]
        public int PatchStatus(int id, [FromBody] int newStatus)
        {
            return _repository.PatchStatus(id, newStatus);
        }

        [HttpDelete("{id:int}")]
        public int Delete(int id)
        {
            return _repository.RemoveOrder(id);
        }
    }
}
