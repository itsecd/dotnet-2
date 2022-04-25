using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TelegramBotServer.Model;
using TelegramBotServer.Repository;


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
        public void Put(int id, [FromBody] Event someEvent)
        {
            _repository.ChangeEvent(id, someEvent);
        }

        // DELETE api/<EventController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repository.RemoveEvent(id);
        }
    }
}
