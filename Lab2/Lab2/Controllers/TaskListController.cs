using Microsoft.AspNetCore.Mvc;
using Lab2.Models;
using Lab2.Repositories;
using System.Collections.Generic;
using System.Linq;

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

        // GET: api/<TaskListController>
        [HttpGet]
        public IEnumerable<TaskList> Get()
        {
            return _taskListRepository.GetTasks();
        }

        // GET api/<TaskListController>/5
        [HttpGet("{id}")]
        public TaskList Get(int id)
        {
            return _taskListRepository.GetTasks().Where(task => task.TaskId == id).Single();

        }

        // POST api/<TaskListController>
        [HttpPost]
        public void Post([FromBody] TaskList value)
        {
            _taskListRepository.AddTask(value);
        }


        // PUT api/<TaskListController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] TaskList value)
        {
            var taskIndex = _taskListRepository.GetTasks().FindIndex(task => task.TaskId == id);

            if (taskIndex > 0)
            {
                _taskListRepository.GetTasks()[taskIndex] = value;
            }
        }

        // DELETE api/<TaskListController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _taskListRepository.RemoveAllTasks();
        }
    }
}
