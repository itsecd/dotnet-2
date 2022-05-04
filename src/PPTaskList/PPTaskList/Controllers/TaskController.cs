using Microsoft.AspNetCore.Mvc;
using PPTask.Controllers.Model;
using PPTask.Repositories;
using System;
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
        public ActionResult<List<Task>> Get()
        {
            try
            {
                return _taskRepository.GetTasks();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Метод получения задачи по идентификатору  
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Задача</returns>
        [HttpGet("{id:int}")]
        public ActionResult <Task> Get(int id)
        {
            try
            {
                return _taskRepository.GetTasks().Where(task => task.TaskId == id).Single();
                if(id < -1) return NotFound();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Метод добавления задачи 
        /// </summary>
        /// <param name="value">Новая задача</param>
        [HttpPost]
        public ActionResult Post([FromBody] Task value)
        {
            try
            {
                _taskRepository.AddTask(new Task {HeaderText = value.HeaderText,TextDescription =value.TextDescription,
                    ExecutorId = value.ExecutorId, TagId = value.TagId });
                return Ok();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Метод замены задачи 
        /// </summary>
        /// <param name="value">Новая задача</param>
        /// /// <param name="id">Идентификатор заменяемой задачи</param>
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] Task value)
        {
            try
            {
                var taskIndex = _taskRepository.GetTasks().FindIndex(task => task.TaskId == id);
                if(taskIndex < -1 || id < -1 ) return NotFound();

                _taskRepository.GetTasks()[taskIndex] = new Task {HeaderText = value.HeaderText,TextDescription =value.TextDescription,
                    ExecutorId = value.ExecutorId, TagId = value.TagId };
                return Ok();

            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Метод удаления задачи 
        /// </summary>
        /// <param name="id">Идентификатор удаляемой задачи</param>
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var taskIndex = _taskRepository.GetTasks().FindIndex(task => task.TaskId == id);
                if(taskIndex < -1) return NotFound();

                _taskRepository.RemoveTask(id);
                return Ok();
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
        [HttpGet("{id:int}/tasks")]
        public ActionResult<List<Task>> GetTasks(int id)
        {
            try
            {
                if(id < -1) return NotFound();
                return _taskRepository.GetTasks().Result.FindAll(task => task.ExecutorId == id);
            }
            catch
            {
                return Problem();
            }
        }
    }
}
