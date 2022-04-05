using AccountingSystem.Model;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public IList<Customer> Get()
        {
            return _repository.GetCustomers();
        }

        /// <summary>Get Customer By ID</summary>
        [HttpGet("{id:int}")]
        public ActionResult<Customer> Get(int id)
        {
            return _repository.GetCustomer(id);
        }

        /// <summary>Add Customer To DataBase</summary>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Customer customer)
        {
            try
            {
                return _repository.AddCustomer(customer);
            }
            catch
            {
                return Conflict();
            }

        }

        /// <summary>Change Customer In DataBase</summary>
        [HttpPut("{id:int}")]
        public ActionResult<int> Put(int id, [FromBody] Customer customer)
        {
            try
            {
                return _repository.ChangeCustomer(id, customer);
            }
            catch
            {
                return Conflict();
            }
        }

        /// <summary>Delete Customer From DataBase</summary>
        [HttpDelete("{id:int}")]
        public ActionResult<int> Delete(int id)
        {
            try 
            {
                return _repository.RemoveCustomer(id);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
