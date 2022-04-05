using AccountingSystem.Model;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        /// <returns>All Order</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IList<Order> Get()
        {
            return _repository.GetOrders();
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
            catch (TypeInitializationException)
            {
                return Forbid();
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
            catch (TypeInitializationException)
            {
                return Forbid();
            }
            catch
            {
                return Conflict();
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
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (TypeInitializationException)
            {
                return Forbid();
            }
            catch
            {
                return Conflict();
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
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (TypeInitializationException)
            {
                return Forbid();
            }
            catch
            {
                return Conflict();
            }
        }

        /// <summary>Change Order Status</summary>
        /// <returns>Order ID</returns>
        [HttpPatch("status-{id:int}")]
        public ActionResult<int> PatchStatus(int id, [FromBody] Order order)
        {
            try
            {
                return _repository.PatchStatus(id, order);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (TypeInitializationException)
            {
                return Forbid();
            }
            catch
            {
                return Conflict();
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
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (TypeInitializationException)
            {
                return Forbid();
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
