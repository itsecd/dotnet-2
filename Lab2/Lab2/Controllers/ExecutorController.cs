using Microsoft.AspNetCore.Mvc;
using Lab2.Models;
using Lab2.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutorController : ControllerBase
    {

        private readonly IExecutorRepository _executorRepository;

        public ExecutorController(IExecutorRepository executorRepository)
        {
            _executorRepository = executorRepository;
        }

        // GET: api/<ExecutorController>
        [HttpGet]
        public IEnumerable<Executor> Get()
        {
            return _executorRepository.GetExecutors();
        }

        // GET api/<ExecutorController>/5
        [HttpGet("{id:int}")]
        public Executor Get(int id)
        {
            return _executorRepository.GetExecutors().Where(executor => executor.ExecutorId == id).Single();
        }

        // POST api/<ExecutorController>
        [HttpPost]
        public void Post([FromBody] Executor value)
        {
            _executorRepository.AddExecutor(value);
        }

        // PUT api/<ExecutorController>/5
        [HttpPut("{id:int}")]
        public void Put(int id, [FromBody] Executor value)
        {
            var executorIndex = _executorRepository.GetExecutors().FindIndex(executor => executor.ExecutorId == id);

            if (executorIndex > 0)
            {
                _executorRepository.GetExecutors()[executorIndex] = value;
            }
        }

        // DELETE api/<ExecutorController>/5
        [HttpDelete]
        public void Delete()
        {
            _executorRepository.RemoveAllExecutors();
        }
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _executorRepository.RemoveExecutor(id);
        }
    }
}
