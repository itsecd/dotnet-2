using AccountingSystem.Exeption;
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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;

        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        /// <summary>Get All Customer</summary>
        /// <returns>All Customer</returns>
        [HttpGet]
        public ActionResult<IList<Customer>> Get()
        {
            try
            {
                return _repository.GetCustomers().ToList();
            }
            catch (TypeInitializationException)
            {
                return Problem();
            }
        }

        /// <summary>Get Customer By ID</summary>
        /// <returns>Customer</returns>
        [HttpGet("{id:int}")]
        public ActionResult<Customer> Get(int id)
        {
            try
            {
                return _repository.GetCustomer(id);
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

        /// <summary>Add Customer To DataBase</summary>
        /// <returns>Customer ID</returns>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Customer customer)
        {
            try
            {
                return _repository.AddCustomer(customer);
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

        /// <summary>Change Customer In DataBase</summary>
        /// <returns>Customer ID</returns>
        [HttpPut("{id:int}")]
        public ActionResult<int> Put(int id, [FromBody] Customer customer)
        {
            try
            {
                return _repository.ChangeCustomer(id, customer);
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

        /// <summary>Delete Customer From DataBase</summary>
        /// <returns>Customer ID</returns>
        [HttpDelete("{id:int}")]
        public ActionResult<int> Delete(int id)
        {
            try 
            {
                return _repository.RemoveCustomer(id);
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
