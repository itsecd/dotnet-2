using System.Collections.Generic;
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
        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;
        }
        // GET: api/<CustomerController>
        [HttpGet]
        public  IEnumerable<Customer> GetAllAsync() 
        {
            return _repository.GetAllCustomers();
        }
        // GET api/<CustomerController>/
        [HttpGet("{id}")]
        public Customer GetAsync(int id) 
        {
            return _repository.GetCustomer(id);
        }

        // POST api/<CustomerController>
        [HttpPost]
        public void Post([FromBody] Customer customer) 
        {
             _repository.AddCustomer(customer);
        }

        // PUT api/<CustomerController>/5

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Customer customer)
        {
            _repository.ReplaceCustomer(id, customer);
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
