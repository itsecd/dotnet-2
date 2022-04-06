﻿using AccountingSystem.Exeption;
using AccountingSystem.Model;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
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
            catch (TypeInitializationException)
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
            catch (NoFoundInDataBaseExeption)
            {
                return NotFound();
            }
            catch (TypeInitializationException)
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
            catch (TypeInitializationException)
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
            catch (TypeInitializationException)
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
            catch (TypeInitializationException)
            {
                return Problem();
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
            catch (NoFoundInDataBaseExeption)
            {
                return NotFound();
            }
            catch (TypeInitializationException)
            {
                return Problem();
            }
            catch
            {
                return Conflict();
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
            catch (NoFoundInDataBaseExeption)
            {
                return NotFound();
            }
            catch (TypeInitializationException)
            {
                return Problem();
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
            catch (NoFoundInDataBaseExeption)
            {
                return NotFound();
            }
            catch (TypeInitializationException)
            {
                return Problem();
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
