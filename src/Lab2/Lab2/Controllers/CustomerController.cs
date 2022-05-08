using System;
using System.Collections.Generic;
using Lab2.Exeptions;
using Lab2.Model;
using Lab2.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers
{
    /// <summary>
    /// Сlass responsible for processing incoming requests and performing operations on customers
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;
        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Getting all customers
        /// </summary>
        /// <returns>All customers</returns>
        [HttpGet]
        public IEnumerable<Customer> GetAll() 
        {
            return _repository.GetAllCustomers();
        }
        /// <summary>
        /// Getting customer by ID
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Customer</returns>
        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id) 
        {
            try
            {
                return _repository.GetCustomer(id);
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
        /// <summary>
        /// Adding customer
        /// </summary>
        /// <param name="customer">Added customer</param>
        [HttpPost]
        public void Post([FromBody] Customer customer) 
        {
             _repository.AddCustomer(customer);
        }
        /// <summary>
        /// Changing the customer by ID
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <param name="customer">Changeable customer</param>
        /// <returns>Customer ID</returns>
        [HttpPut("{id}")]
        public ActionResult<int> Put(int id, [FromBody] Customer customer)
        {
            try
            {
               return _repository.ReplaceCustomer(id, customer);
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
        /// <summary>
        /// Deleting customer by ID
        /// </summary>
        /// <param name="id">ID of the customer being deleted</param>
        /// <returns>Customer ID</returns>
        [HttpDelete("{id}")]
        public ActionResult<int> Delete(int id) 
        {
            try
            {
                return _repository.DeleteCustomer(id);
            }
            catch(NotFoundException)
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
        /// <summary>
        /// Deleting all customers
        /// </summary>
        [HttpDelete]
        public void DeleteAll()
        {
            _repository.DeleteAllCustomers();
        }
    }
}
