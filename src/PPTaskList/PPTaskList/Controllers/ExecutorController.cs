using Microsoft.AspNetCore.Mvc;
using PPTask.Controllers.Model;
using PPTask.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PPTask.Controllers
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

        [HttpGet]
        public IEnumerable<Executor> Get()
        {
            return _executorRepository.GetExecutors();
        }

        [HttpGet("{id}")]
        public Executor Get(int id)
        {
            return _executorRepository.GetExecutors().Where(executor => executor.ExecutorId == id).Single();
        }

        [HttpPost]
        public void Post([FromBody] Executor value)
        {
            _executorRepository.AddExecutor(value);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Executor value)
        {
            var executorIndex = _executorRepository.GetExecutors().FindIndex(executor => executor.ExecutorId == id);
            
            if(executorIndex > 0)
            {
                _executorRepository.GetExecutors()[executorIndex] = value;
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _executorRepository.RemoveAllExecutors();
        }
    }
}
