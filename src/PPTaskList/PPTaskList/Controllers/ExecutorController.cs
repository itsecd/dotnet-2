using Microsoft.AspNetCore.Mvc;
using PPTaskList.Controllers.Model;
using System.Collections.Generic;

namespace PPTaskList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutorController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Executor> Get()
        {
            return new Executor[]{
                        new Executor(0, "Tom"),
                        new Executor(1, "Mike"),
            };
        }

        [HttpGet("{id}")]
        public Executor Get(int id)
        {
            return new Executor(0, "Tom");
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
