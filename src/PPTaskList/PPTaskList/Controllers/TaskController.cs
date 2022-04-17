using Microsoft.AspNetCore.Mvc;
using PPTask.Controllers.Model;
using PPTask.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PPTask.Controllers
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

        [HttpGet]
        public IEnumerable<Task> Get()
        {
            return _taskListRepository.GetTasks();
        }

        [HttpGet("{id}")]
        public Task Get(int id)
        {
            return _taskListRepository.GetTasks().Where(task => task.TaskId == id).Single();

        }

        [HttpPost]
        public void Post([FromBody] Task value)
        {
            _taskListRepository.AddTask(value);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Task value)
        {
            var taskIndex = _taskListRepository.GetTasks().FindIndex(task => task.TaskId == id);

            if (taskIndex > 0)
            {
                _taskListRepository.GetTasks()[taskIndex] = value;
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _taskListRepository.RemoveAllTasks();
        }
    }
}
