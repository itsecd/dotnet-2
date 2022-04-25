using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TelegramBotServer.Model;
using TelegramBotServer.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TelegramBotServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriberController : ControllerBase
    {
        private readonly ISubscriberRepository _repository;
        public SubscriberController(ISubscriberRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<SubscriberController>
        [HttpGet]
        public IEnumerable<Subscriber> Get()
        {
            return _repository.GetSubscribers();
        }

        // GET api/<SubscriberController>/5
        [HttpGet("{id}")]
        public Subscriber Get(int id)
        {
            return _repository.GetSubscriber(id);
        }

        // POST api/<SubscriberController>
        [HttpPost]
        public void Post([FromBody] Subscriber newSub)
        {
            _repository.AddSubscriber(newSub);
        }

        // PUT api/<SubscriberController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Subscriber newSub)
        {
            _repository.ChangeSubscriber(id, newSub);
        }

        // DELETE api/<SubscriberController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repository.RemoveSubscriber(_repository.GetSubscriber(id));
        }
    }
}
