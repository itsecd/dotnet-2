using Microsoft.AspNetCore.Mvc;
using Lab2.Models;
using Lab2.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;
using Lab2.Exceptions;
using System.Threading.Tasks;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskListController : ControllerBase
    {
        private readonly ITaskRepository _taskListRepository;

        public TaskListController(ITaskRepository taskListRepository)
        {
            _taskListRepository = taskListRepository;
        }
        /// <summary>
        /// Получение всех задач
        /// </summary>
        /// <returns>All Tasks</returns>
        // GET: api/<TaskListController>
        [HttpGet]
        public async Task<ActionResult<List<TaskList>>> Get() => await _taskListRepository.GetTasks();
       
        /// <summary>
        /// Получение задачи по индентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Task</returns>
        // GET api/<TaskListController>/5
        [HttpGet("{id:int}")]
        public ActionResult<TaskList> Get(int id)
        {

            try
            {
                var task = _taskListRepository.GetTasks().Result.Where(task => task.TaskId == id).Single();
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
        public IActionResult Post([FromBody] TaskList task)
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
        public IActionResult Put(int id, [FromBody] TaskList task)
        {

            try
            {
                var taskIndex = _taskListRepository.GetTasks().Result.FindIndex(task => task.TaskId == id);
                _taskListRepository.GetTasks().Result[taskIndex] = task;
                _taskListRepository.SaveFile();
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

