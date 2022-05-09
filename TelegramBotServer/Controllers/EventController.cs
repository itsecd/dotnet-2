using Microsoft.AspNetCore.Mvc;
using System;
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

        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns>List of all events</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Event>> Get()
        {
            var events = _repository.GetEvents();
            if (events is not null)
                return new ActionResult<IEnumerable<Event>>(events);
            else
                return Problem("Events from repository is null!");
        }

        /// <summary>
        /// Get event with specific id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Event or 404 code if not found</returns>
        [HttpGet("{id}")]
        public ActionResult<Event> Get(int id)
        {
            var someEvent = _repository.GetEvent(id);
            if (someEvent is null)
                return NotFound($"Event with id {id} not found");
            else
                return someEvent;
        }

        /// <summary>
        /// Add new event
        /// </summary>
        /// <param name="someEvent"></param>
        /// <returns>Id inserted event</returns>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Event someEvent)
        {
            try
            {
                return _repository.AddEvent(someEvent);
                
            }
            catch (Exception exc)
            {
                return Problem($"Problem detected: {exc.Message}");
            }
            
        }

        /// <summary>
        /// Replace event with specific id by new event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newEvemt"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Event newEvemt)
        {
            try
            {
                if (_repository.ChangeEvent(id, newEvemt))
                    return Ok();
                else
                    return NotFound($"Event with id {id} not found");
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
                if (_repository.RemoveEvent(id))
                    return Ok();
                else
                    return NotFound($"Event with id {id} not found");
            }
            catch (Exception exc)
            {
                return Problem($"Problem detected: {exc.Message}");
            }            
        }
    }
}
