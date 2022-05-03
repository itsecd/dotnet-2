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
                return _taskRepository.GetTasks().Result;
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
        [HttpGet("{id}")]
        public ActionResult <Task> Get(int id)
        {
            try
            {
                return _taskRepository.GetTasks().Result.Where(task => task.TaskId == id).Single();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
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
        public ActionResult<int> Post([FromBody] Task value)
        {
            try
            {
                _taskRepository.AddTask(value);
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
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Task value)
        {
            try
            {
                var taskIndex = _taskRepository.GetTasks().Result.FindIndex(task => task.TaskId == id);
                _taskRepository.GetTasks().Result[taskIndex] = value;
                return Ok();

            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
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
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _taskRepository.RemoveTask(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
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
        [HttpGet("{id}/tasks")]
        public ActionResult<List<Task>> GetTasks(int id)
        {
            try
            {
                return _taskRepository.GetTasks().Result.FindAll(task => task.ExecutorId == id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }
        }
    }
}
