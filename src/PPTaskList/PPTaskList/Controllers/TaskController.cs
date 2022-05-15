using Microsoft.AspNetCore.Mvc;
using PPTask.Model;
using PPTask.Repositories;
using PPTask.Dto;
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
        /// Репозиторий исполнителей
        /// </summary>
        private readonly IExecutorRepository _executorRepository;

        /// <summary>
        /// Конструктор с параметрами. В качестве параметра принимает репозиторий.
        /// </summary>
        public TaskController(ITaskRepository taskRepository, IExecutorRepository executorRepository)
        {
            _taskRepository = taskRepository;
            _executorRepository = executorRepository;
        }

        /// <summary>
        /// Метод получения задач
        /// </summary>
        /// <returns>Теги</returns>
        [HttpGet]
        public ActionResult<List<TaskDto>> Get()
        {
            try
            {
                var tasks = new List <TaskDto>();
                foreach(Task task in _taskRepository.GetTasks())
                {
                    tasks.Add(new TaskDto
                    {
                        HeaderText = task.HeaderText,
                        TextDescription = task.TextDescription,
                        TagsId = task.TagsId,
                        Executor = _executorRepository.GetExecutors().Single(
                            executor => executor.ExecutorId == task.ExecutorId)
                    });
                }
                return tasks;
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
        public ActionResult <TaskDto> Get(int id)
        {
            try
            {
                if(id < -1) return NotFound();
                var task = _taskRepository.GetTasks().Single(task => task.TaskId == id);
                var taskDto = new TaskDto { 
                    HeaderText = task.HeaderText,
                    TextDescription = task.TextDescription,
                    TagsId = task.TagsId,
                    Executor = _executorRepository.GetExecutors().Single(
                        executor => executor.ExecutorId == task.ExecutorId)
                };
                return taskDto;
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
        public ActionResult Post([FromBody] TaskDto value)
        {
            try
            {
                _taskRepository.AddTask(new Task {
                    HeaderText = value.HeaderText,
                    TextDescription = value.TextDescription,
                    ExecutorId = value.Executor.ExecutorId,
                    TagsId = value.TagsId 
                });
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
        public ActionResult Put(int id, [FromBody] TaskDto value)
        {
            try
            {
                var taskIndex = _taskRepository.GetTasks().FindIndex(task => task.TaskId == id);
                if(taskIndex < -1 || id < -1 ) return NotFound();

                _taskRepository.GetTasks()[taskIndex] = new Task {
                    HeaderText = value.HeaderText,
                    TextDescription = value.TextDescription,
                    ExecutorId = value.Executor.ExecutorId,
                    TagsId = value.TagsId };
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
    }
}
