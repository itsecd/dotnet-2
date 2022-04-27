using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2.Model;
using Lab2.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace Lab2.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(ICustomerRepository repository, ILogger<CustomerController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        // GET: api/<CustomerController>
        [HttpGet]
        public  IEnumerable<Customer> GetAllAsync() 
        {
            _logger.LogInformation("Method get is invoked!");
            return _repository.GetAllCustomers();
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public Customer GetAsync(int id) 
        {
            return _repository.GetCustomer(id);
        }

        // POST api/<CustomerController>
        [HttpPost]
        public void Post([FromBody] Customer customer) 
        {
            _logger.LogWarning("Method post is invoked!");
             _repository.AddCustomer(customer);
            
        }

        // PUT api/<CustomerController>/5

        [HttpPut("{id}")]
        public void Put([FromBody] Customer customer, int id)
        {
            _repository.ReplaceCustomer(customer, id);
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) 
        {
            _repository.DeleteCustomer(id);
        }
        
        [HttpDelete]
        public async Task DeleteAll()
        {
            _repository.DeleteAllCustomers();

        }
    }
}
