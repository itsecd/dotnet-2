using Microsoft.AspNetCore.Mvc;
using PPTask.Controllers.Model;
using PPTask.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PPTask.Controllers
{
    /// <summary>
    /// Контроллер исполнителей задач
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutorController : ControllerBase
    {
        /// <summary>
        /// Репозиторий исполнителей
        /// </summary>
        private readonly IExecutorRepository _executorRepository;

        /// <summary>
        /// Конструктор с параметрами. В качестве параметра принимает репозиторий.
        /// </summary>
        public ExecutorController(IExecutorRepository executorRepository)
        {
            _executorRepository = executorRepository;
        }

        /// <summary>
        /// Метод получения исполнителей
        /// </summary>
        /// <returns>Исполнители</returns>
        [HttpGet]
        public IEnumerable<Executor> Get()
        {
            return (IEnumerable<Executor>)_executorRepository.GetExecutors().Result;
        }

        /// <summary>
        /// Метод получения исполнителя по идентификатору 
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        /// <returns>Исполнитель</returns>
        [HttpGet("{id}")]
        public Executor Get(int id)
        {
            return _executorRepository.GetExecutors().Result.Where(executor => executor.ExecutorId == id).Single();
        }

        /// <summary>
        /// Метод добавления исполнителя 
        /// </summary>
        /// <param name="value">Новый исполнитель</param>
        [HttpPost]
        public void Post([FromBody] Executor value)
        {
            _executorRepository.AddExecutor(value);
        }

        /// <summary>
        /// Метод замены исполнителя 
        /// </summary>
        /// <param name="value">Новый исполнитель</param>
        /// /// <param name="id">Идентификатор заменяемого исполнителя</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Executor value)
        {
            var executorIndex = _executorRepository.GetExecutors().Result.FindIndex(executor => executor.ExecutorId == id);
            
            if(executorIndex > 0)
            {
                _executorRepository.GetExecutors().Result[executorIndex] = value;
            }
        }

        /// <summary>
        /// Метод удаления исполнителя 
        /// </summary>
        /// <param name="id">Идентификатор удаляемого исполнителя</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _executorRepository.RemoveExecutor(id);
        }
    }
}
