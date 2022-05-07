using System;
using System.Collections.Generic;
using Lab2.Exeptions;
using Lab2.Model;
using Lab2.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers
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
        [HttpGet]
        public IEnumerable<Customer> GetAll() 
        {
            return _repository.GetAllCustomers();
        }
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
        [HttpPost]
        public void Post([FromBody] Customer customer) 
        {
             _repository.AddCustomer(customer);
        }
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
        [HttpDelete]
        public void DeleteAll()
        {
            _repository.DeleteAllCustomers();
        }
    }
}
