using Lab2.Model;
using Lab2.NotFoundDataException;
using Lab2.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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

        /// <summary>
        ///  Получение списка покупателей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Customer> GetListCustomer() => _repository.ListCustomers();

        /// <summary>
        /// Метод получения клиента
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id)
        {
            try
            {
                return _repository.GetCustomer(id);
            }
            catch (NotFoundException)
            {
                return NotFound("ID does not exist!");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("ID invalid!");
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Добавление покупатели
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Customer customer)
        {
            try
            {
                return _repository.Add(customer);
            }
            catch (ArgumentNullException)
            {
                return Problem("Data of customer is empty!");
            }
            catch (ArgumentException e)
            {
                if (e.Message == "ID exist!")
                    return BadRequest("Customer ID exist!");
                if (e.Message == "ID invalid!") return BadRequest("ID invalid");
                return Problem();
            }
        }

        /// <summary>
        /// редактирование покупатель по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newCustomer"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<int> Put(int id, [FromBody] Customer newCustomer)
        {
            try
            {
                return _repository.Change(id, newCustomer);
            }
            catch (NotFoundException)
            {
                return NotFound("ID does not exist!");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("ID invalid!");
            }
            catch (ArgumentNullException)
            {
                return Problem("Data is empty!");
            }
        }

        /// <summary>
        /// Удаление покупатели по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult<int> Delete(int id)
        {
            try
            {
                return _repository.Remove(id);
            }
            catch (NotFoundException)
            {
                return NotFound("ID does not exist!");
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("ID invlaid!");
            }
            catch
            {
                return Problem();
            }
        }
    }
}
