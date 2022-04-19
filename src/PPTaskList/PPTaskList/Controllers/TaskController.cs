using Microsoft.AspNetCore.Mvc;
using PPTask.Controllers.Model;
using PPTask.Repositories;
using System.Collections.Generic;
using System.Linq;


namespace PPTask.Controllers
{
    /// <summary>
    /// Контроллер задач
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        /// <summary>
        /// Репозиторий задач
        /// </summary>
        private readonly ITaskRepository _taskRepository;

        /// <summary>
        /// Конструктор с параметрами. В качестве параметра принимает репозиторий.
        /// </summary>
        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        /// <summary>
        /// Метод получения задач
        /// </summary>
        /// <returns>Теги</returns>
        [HttpGet]
        public IEnumerable<Task> Get()
        {
            return (IEnumerable<Task>)_taskRepository.GetTasks().Result;
        }

        /// <summary>
        /// Метод получения задачи по идентификатору  
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Задача</returns>
        [HttpGet("{id}")]
        public Task Get(int id)
        {
            return _taskRepository.GetTasks().Result.Where(task => task.TaskId == id).Single();
        }

        /// <summary>
        /// Метод добавления задачи 
        /// </summary>
        /// <param name="value">Новая задача</param>
        [HttpPost]
        public void Post([FromBody] Task value)
        {
            _taskRepository.AddTask(value);
        }

        /// <summary>
        /// Метод замены задачи 
        /// </summary>
        /// <param name="value">Новая задача</param>
        /// /// <param name="id">Идентификатор заменяемой задачи</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Task value)
        {
            var taskIndex = _taskRepository.GetTasks().Result.FindIndex(task => task.TaskId == id);

            if (taskIndex > 0)
            {
                _taskRepository.GetTasks().Result[taskIndex] = value;
            }
        }

        /// <summary>
        /// Метод удаления задачи 
        /// </summary>
        /// <param name="id">Идентификатор удаляемой задачи</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _taskRepository.RemoveTask(id);
        }

        /// <summary>
        /// Метод получения всех задач для конкретного исполнителя 
        /// </summary>
        /// <returns>Задачи</returns>
        [HttpGet("{id}/tasks")]
        public List<Task> GetTasks(int id)
        {
            return _taskRepository.GetTasks().Result.FindAll(task => task.ExecutorId == id);
        }
    }
}
