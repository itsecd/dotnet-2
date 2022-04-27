using Microsoft.AspNetCore.Mvc;
using Server.Model;
using Server.Repositories;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEventController : ControllerBase
    {
        private readonly IJSONRepository<UserEvent> _userEventRepository;

        public UserEventController(IJSONRepository<UserEvent> userEventRepository) => _userEventRepository = userEventRepository;

        // GET: api/<UserEventController>
        [HttpGet]
        public IEnumerable<UserEvent> Get()
        {
            return _userEventRepository.Get();
        }

        // GET api/<UserEventController>/5
        [HttpGet("{id}")]
        public UserEvent Get(int id)
        {
            return _userEventRepository.Get(id);
        }

        // POST api/<UserEventController>
        [HttpPost]
        public void Post([FromBody] UserEvent userEvent)
        {
            _userEventRepository.Add(userEvent);
        }

        // PUT api/<UserEventController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] UserEvent userEvent)
        {
            _userEventRepository.Update(id, userEvent);
        }

        // DELETE api/<UserEventController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _userEventRepository.Delete(id);
        }

        [HttpDelete]
        public void Delete()
        {
            _userEventRepository.DeleteAll();
        }
    }
}
