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

        [HttpGet]
        public IList<Customer> Get()
        {
            return _repository.GetCustomers();
        }

        [HttpGet("{id}")]
        public Customer Get(int id)
        {
            return _repository.GetCustomer(id);
        }

        [HttpPost]
        public int Post([FromBody] Customer customer)
        {
            return _repository.AddCustomer(customer);
        }

        [HttpPut("{id}")]
        public int Put(int id, [FromBody] Customer customer)
        {
            return _repository.ChangeCustomer(id, customer);
        }

        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            return _repository.RemoveCustomer(id);
        }
    }
}
