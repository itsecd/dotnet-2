using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Server.Model;
using Server.Repositories;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJSONRepository<User> _userRepositiry;
        private readonly List<Task> tasks;
        public UserController(IJSONRepository<User> userRepository) => _userRepositiry = userRepository;
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _userRepositiry.Get();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return _userRepositiry.Get(id);
        }

        [HttpGet("byName={name}")]
        public User Get(string name)
        {
            return _userRepositiry.Get(name);
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] User user)
        {
            _userRepositiry.Add(user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User user)
        {
            if (_userRepositiry.Update(id, user))
            {
                HttpContext.Response.StatusCode = 200;
            }
            else HttpContext.Response.StatusCode = 404;
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _userRepositiry.Delete(id);
        }

        [HttpDelete]
        public void Delete()
        {
            _userRepositiry.DeleteAll();
        }
    }
}
