using Microsoft.AspNetCore.Mvc;
using PPTaskList.Controllers.Model;
using PPTaskList.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PPTaskList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskListRepository _taskListRepository;

        public TaskController(ITaskListRepository taskListRepository)
        {
            _taskListRepository = taskListRepository;
        }

        [HttpGet]
        public IEnumerable<TaskList> Get()
        {
            return _taskListRepository.GetTasks();
        }

        [HttpGet("{id}")]
        public TaskList Get(int id)
        {
            return _taskListRepository.GetTasks().Where(task => task.TaskId == id).Single();

        }

        [HttpPost]
        public void Post([FromBody] TaskList value)
        {
            _taskListRepository.AddTask(value);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] TaskList value)
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
