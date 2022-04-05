using Microsoft.AspNetCore.Mvc;
using PPTaskList.Controllers.Model;
using System.Collections.Generic;

namespace PPTaskList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<TaskList> Get()
        {
            return new TaskList[]{
                        new TaskList("task1", "description1"),
                        new TaskList("task2", "description2")
            };
        }

        [HttpGet("{id}")]
        public TaskList Get(int id)
        {
            return new TaskList("task1", "description1");
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
