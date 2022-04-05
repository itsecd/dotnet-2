using AccountingSystem.Model;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Http;
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

        /// <summary>Get All Order</summary>
        [HttpGet]
        public IList<Order> Get()
        {
            return _repository.GetOrders();
        }

        /// <summary>Get Order By ID</summary>
        [HttpGet("{id:int}")]
        public ActionResult<Order> Get(int id)
        {
            return _repository.GetOrder(id);
        }

        /// <summary>Get All Price By Order</summary>
        [HttpGet("all-price")]
        public double GetAllPrice()
        {
            return _repository.GetAllPrice();
        }

        /// <summary>Add Order To DataBase</summary>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Order order)
        {
            try
            {
                return _repository.AddOrder(order);
            }            
            catch
            {
                return Conflict();
            }

        }

        /// <summary>Change Order In DataBase</summary>
        [HttpPut("{id:int}")]
        public ActionResult<int> Put(int id, [FromBody] Order order)
        {
            try
            {
                return _repository.ChangeOrder(id, order);
            }
            catch
            {
                return Conflict();
            }
        }

        /// <summary>Change Order Status</summary>
        [HttpPatch("status-{status:int}")]
        public ActionResult<int> PatchStatus(int status, [FromBody] Order order)
        {
            try
            {
                return _repository.PatchStatus(status, order);
            }
            catch
            {
                return Conflict();
            }

        }

        /// <summary>Delete Order From DataBase</summary>
        [HttpDelete("{id:int}")]
        public ActionResult<int> Delete(int id)
        {
            try
            {
                return _repository.RemoveOrder(id);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
