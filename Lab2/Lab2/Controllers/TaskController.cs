using Microsoft.AspNetCore.Mvc;
using Lab2.Models;
using Lab2.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;
using Lab2.Exceptions;


namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskListRepository;

        public TaskController(ITaskRepository taskListRepository)
        {
            _taskListRepository = taskListRepository;
        }
        /// <summary>
        /// Получение всех задач
        /// </summary>
        /// <returns>All Tasks</returns>
        // GET: api/<TaskListController>
        [HttpGet]
        public ActionResult<List<Task>> Get() => _taskListRepository.GetTasks();
       
        /// <summary>
        /// Получение задачи по индентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Task</returns>
        // GET api/<TaskListController>/5
        [HttpGet("{id:int}")]
        public ActionResult<Task> Get(int id)
        {

            try
            {
                var task = _taskListRepository.GetTasks().Single(task => task.TaskId == id);
                return task;
            }
            catch (NotFoundException)
            {
                return NotFound();
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
        // POST api/<TaskListController>
        [HttpPost]
        public IActionResult Post([FromBody] Task task)
        {

            try
            {
                _taskListRepository.AddTask(task);
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
        // PUT api/<TaskListController>/5
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Task task)
        {

            try
            {
                _taskListRepository.UpdateTask(id, task);
                return Ok();
            }
            catch (NotFoundException)
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
        /// Удаление всех задач
        /// </summary>
        // DELETE api/<TaskListController>/5
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
        // DELETE api/<TaskListController>/5
        [HttpDelete("{id:int}")]
       
        public IActionResult Delete(int id)
        {
            try
            {
                _taskListRepository.RemoveTask(id);
                return Ok();
            }
            catch (NotFoundException)
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

