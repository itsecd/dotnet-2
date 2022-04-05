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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IList<Customer> Get()
        {
            return _repository.GetCustomers();
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
            catch (TypeInitializationException)
            {
                return Forbid();
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

        /// <summary>Change Customer In DataBase</summary>
        /// <returns>Customer ID</returns>
        [HttpPut("{id:int}")]
        public ActionResult<int> Put(int id, [FromBody] Customer customer)
        {
            try
            {
                return _repository.ChangeCustomer(id, customer);
            }
            catch(NullReferenceException)
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

        /// <summary>Delete Customer From DataBase</summary>
        /// <returns>Customer ID</returns>
        [HttpDelete("{id:int}")]
        public ActionResult<int> Delete(int id)
        {
            try 
            {
                return _repository.RemoveCustomer(id);
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
