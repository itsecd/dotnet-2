using Lab2.Models;
using Lab2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;


namespace Lab2.Controllers
{
    /// <summary>
    /// Контроллер для задач
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        /// <summary>
        /// Репозиторий задач
        /// </summary>
        private readonly ITaskRepository _taskListRepository;

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        public TaskController(ITaskRepository taskListRepository)
        {
            _taskListRepository = taskListRepository;
        }

        /// <summary>
        /// Получение всех задач
        /// </summary>
        /// <returns>All Tasks</returns>
        [HttpGet]
        public ActionResult<List<Task>> Get() => _taskListRepository.GetTasks();

        /// <summary>
        /// Получение задачи по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Task</returns>
        [HttpGet("{id:int}")]
        public ActionResult<Task> Get(int id)
        {
            try
            {
                if (id < -1)
                {
                    return NotFound();
                }
                var task = _taskListRepository.Get(id);
                return task;
            }
            catch
            {
                return Problem();
            }
        }


        /// <summary>
        /// Добавление задачи
        /// </summary>
        /// <param name="task">Новая задача</param>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Task task)
        {

            try
            {
                return _taskListRepository.AddTask(task);
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Изменение параметров задачи по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="task">Новая задача</param>
        [HttpPut("{id:int}")]
        public ActionResult<int> Put(int id, [FromBody] Task task)
        {

            try
            {
                return _taskListRepository.UpdateTask(id, task);
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
        /// Удаление всех задач
        /// </summary>
        [HttpDelete]
        public void Delete()
        {
            _taskListRepository.RemoveAllTasks();
        }

        /// <summary>
        /// Удаление задачи по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        [HttpDelete("{id:int}")]

        public ActionResult<int> Delete(int id)
        {
            try
            {
                return _taskListRepository.RemoveTask(id);
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
        /// все тэги по идентификатору
        /// </summary>
        /// <param name="id"> ID</param>
        /// <returns>все тэги</returns>
        [HttpGet("{id}/Tags")]
        public ActionResult<List<Tags>> GetTags(int id)
        {
            try
            {
                return _taskListRepository.GetTags(id);
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
        /// Добавление тэга
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="tag">Новый тэг</param>
        /// <returns>Order ID</returns>
        [HttpPost("{id}/Tags")]
        public ActionResult<int> PostProduct(int id, Tags tag)
        {
            try
            {
                return _taskListRepository.AddTag(id, tag);
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
        /// Получение тэга, связанного с задачей по его номеру
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="num">Номер тэга</param>
        /// <returns>Tag</returns>
        [HttpGet("{id}/Tags/{num}")]
        public ActionResult<Tags> GetProduct(int id, int num)
        {
            try
            {
                return _taskListRepository.GetTag(id, num);
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
        /// Удаление тэга, свяазнного с задачей по номеру
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="num">Номер тэга</param>
        /// <returns>TaskId </returns>
        [HttpDelete("{id}/Tags/{num}")]
        public ActionResult<int> DeleteProduct(int id, int num)
        {
            try
            {
                return _taskListRepository.RemoveTag(id, num);
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
        /// Изменение тэга
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="num">Номер тэга</param>
        /// <param name="tag">Новый тэг</param>
        /// <returns>Order ID</returns>
        [HttpPut("{id}/Tags/{num}")]
        public ActionResult<int> PutProduct(int id, int num, Tags tag)
        {
            try
            {
                return _taskListRepository.UpdateTag(id, num, tag);
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
        /// Удаление всех тэгов, связанных с задачей
        /// </summary>
        /// <param name="id">TaskId</param>
        /// <returns>TaskId</returns>
        [HttpDelete("{id}/Tags")]
        public ActionResult<int> DeleteAllTags(int id)
        {
            try
            {
                return _taskListRepository.RemoveAllTags(id);
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

