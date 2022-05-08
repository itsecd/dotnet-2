using Microsoft.AspNetCore.Mvc;
using PPTask.Controllers.Model;
using PPTask.Repositories;
using System.Collections.Generic;
using System.Linq;
using PPTask.Dto;

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
        /// Репозиторий задач
        /// </summary>
        private readonly ITaskRepository _taskRepository;

        /// <summary>
        /// Конструктор с параметрами. В качестве параметра принимает репозиторий.
        /// </summary>
        public ExecutorController(IExecutorRepository executorRepository, ITaskRepository taskRepository)
        {
            _executorRepository = executorRepository;
            _taskRepository = taskRepository;
        }

        /// <summary>
        /// Метод получения исполнителей
        /// </summary>
        /// <returns>Исполнители</returns>
        [HttpGet]
        public ActionResult<List<Executor>> Get()
        {
            try
            {
                return _executorRepository.GetExecutors();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Метод получения исполнителя по идентификатору 
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        /// <returns>Исполнитель</returns>
        [HttpGet("{id:int}")]
        public ActionResult<Executor> Get(int id)
        {
            try
            {
                if(id < -1) return NotFound();
                return _executorRepository.GetExecutors().Single(executor => executor.ExecutorId == id);
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Метод получения всех задач для конкретного исполнителя 
        /// </summary>
        /// <returns>Задачи</returns>
        [HttpGet("{id:int}/executor")]
        public ActionResult<List<Task>> GetTasks(int id)
        {
            try
            {
                if(id < -1) return NotFound();
                return _taskRepository.GetTasks().FindAll(task => task.ExecutorId == id);
            }
            catch
            {
                return Problem();
            }
        }
        /// <summary>
        /// Метод добавления исполнителя 
        /// </summary>
        /// <param name="value">Новый исполнитель</param>
        [HttpPost]
        public ActionResult Post([FromBody] ExecutorDto value)
        {
            try
            {
                _executorRepository.AddExecutor(new Executor {Name = value.Name});
                return Ok();
            }
            catch 
            {
                return Problem();
            }
        }

        /// <summary>
        /// Метод замены исполнителя 
        /// </summary>
        /// <param name="value">Новый исполнитель</param>
        /// /// <param name="id">Идентификатор заменяемого исполнителя</param>
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] ExecutorDto value)
        {
            try
            {
                var executorIndex = _executorRepository.GetExecutors().FindIndex(executor => executor.ExecutorId == id);
                if(executorIndex < -1 || id < -1 ) return NotFound();

                _executorRepository.GetExecutors()[executorIndex] = new Executor {Name = value.Name};
                return Ok();

            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Метод удаления исполнителя 
        /// </summary>
        /// <param name="id">Идентификатор удаляемого исполнителя</param>
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var executorIndex = _executorRepository.GetExecutors().FindIndex(executor => executor.ExecutorId == id);
                if(executorIndex < -1) return NotFound();

                _executorRepository.RemoveExecutor(id);
                return Ok();
            }
            catch
            {
                return Problem();
            }
        }
    }
}
