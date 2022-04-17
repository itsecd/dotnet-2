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
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public IEnumerable<Task> Get()
        {
            return (IEnumerable<Task>)_taskRepository.GetTasks();
        }

        [HttpGet("{id}")]
        public Task Get(int id)
        {
            return _taskRepository.GetTasks().Result.Where(task => task.TaskId == id).Single();
        }

        [HttpPost]
        public void Post([FromBody] Task value)
        {
            _taskRepository.AddTask(value);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Task value)
        {
            var taskIndex = _taskRepository.GetTasks().Result.FindIndex(task => task.TaskId == id);

            if (taskIndex > 0)
            {
                _taskRepository.GetTasks().Result[taskIndex] = value;
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _taskRepository.RemoveAllTasks();
        }
    }
}
