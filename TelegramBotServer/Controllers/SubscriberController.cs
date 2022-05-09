using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TelegramBotServer.Model;
using TelegramBotServer.Repository;


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

        /// <summary>
        /// Get all subscribers
        /// </summary>
        /// <returns>List of all subscribers</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Subscriber>> Get()
        {
            var subs = _repository.GetSubscribers();
            if (subs is not null)
                return new ActionResult<IEnumerable<Subscriber>>(subs);
            else
                return Problem("Subscribers from repository is null!");
        }

        /// <summary>
        /// Get subscriber with specific id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Subscriber or 404 code if not found</returns>
        [HttpGet("{id}")]
        public ActionResult<Subscriber> Get(int id)
        {
            var subscriber = _repository.GetSubscriber(id);
            if (subscriber is null)
                return NotFound($"Subscriber with id {id} not found");
            else
                return subscriber;
        }

        /// <summary>
        /// Add new subscriber
        /// </summary>
        /// <param name="newSub"></param>
        /// <returns>Id inserted subscriber</returns>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Subscriber newSub)
        {
            try
            {
                return _repository.AddSubscriber(newSub);
            }
            catch (Exception exc)
            {
                return Problem($"Problem detected: {exc.Message}");
            }            
        }

        /// <summary>
        /// Replace sub with specific id by new event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newSub"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Subscriber newSub)
        {
            try
            {
                if (_repository.ChangeSubscriber(id, newSub))
                    return Ok();
                else
                    return NotFound($"Subscriber with id {id} not found");
            }
            catch (Exception exc)
            {
                return Problem($"Problem detected: {exc.Message}");
            }
        }

        /// <summary>
        /// Delete event by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _repository.RemoveSubscriber(id);
                return Ok();
            }
            catch (Exception exc)
            {
                return Problem($"Problem detected: {exc.Message}");
            }
        }
    }
}
