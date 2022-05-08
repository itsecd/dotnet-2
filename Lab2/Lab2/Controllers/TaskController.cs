using Lab2.Dto;
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
        public IActionResult Post([FromBody] TaskDto task)
        {

            try
            {
                _taskListRepository.AddTask(new Task
                {
                    Name = task.Name,
                    Description = task.Description,
                    ExecutorId = task.ExecutorId,
                    TagsId = task.TagsId
                });
                return CreatedAtAction(nameof(Post), task);
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
        public IActionResult Put(int id, [FromBody] TaskDto task)
        {

            try
            {
                _taskListRepository.UpdateTask(id, new Task
                {
                    TaskId = id,
                    Name = task.Name,
                    Description = task.Description,
                    ExecutorId = task.ExecutorId,
                    TagsId = task.TagsId
                });
                return Ok();
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
        public IActionResult Delete()
        {
            try
            {
                _taskListRepository.RemoveAllTasks();
                return Ok();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Удаление задачи по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        [HttpDelete("{id:int}")]

        public IActionResult Delete(int id)
        {
            try
            {
                _taskListRepository.RemoveTask(id);
                return Ok();
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

