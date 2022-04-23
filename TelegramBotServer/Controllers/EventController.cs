using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotServer.Model;
using TelegramBotServer.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TelegramBotServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _repository;
        public EventController(IEventRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<EventController>
        [HttpGet]
        public IEnumerable<Event> Get()
        {
            return _repository.GetEvents();
        }

        // GET api/<EventController>/5
        [HttpGet("{id}")]
        public Event Get(int id)
        {
            return _repository.GetEvent(id);
        }

        // POST api/<EventController>
        [HttpPost]
        public void Post([FromBody] Event someEvent)
        {
            _repository.AddEvent(someEvent);
        }

        // PUT api/<EventController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EventController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
