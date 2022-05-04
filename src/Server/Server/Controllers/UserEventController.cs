using Microsoft.AspNetCore.Mvc;
using Server.Model;
using Server.Repositories;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEventController : ControllerBase
    {
        private readonly IJSONRepository<UserEvent> _userEventRepository;

        public UserEventController(IJSONRepository<UserEvent> userEventRepository) => _userEventRepository = userEventRepository;


        /// <summary>
        /// Getting all user events
        /// </summary>
        /// <returns>User events</returns>
        /// <response code="200">Success</response>
        /// <response code="404">User events not found</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserEvent>), 200)]
        [ProducesResponseType(typeof(List<UserEvent>), 404)]
        public IEnumerable<UserEvent> Get()
        {
            List<UserEvent> userEvents = (List<UserEvent>)_userEventRepository.Get();
            if (userEvents == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Response.StatusCode = 200;
            return userEvents;
        }

        /// <summary>
        /// Getting a user event by unique id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User event</returns>
        /// <response code="200">Success</response>
        /// <response code="404">A user event with this ID was not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserEvent), 200)]
        [ProducesResponseType(typeof(UserEvent), 404)]
        public UserEvent Get(int id)
        {
            UserEvent userEvent = _userEventRepository.Get(id);
            if (userEvent == null)
            {
                Response.StatusCode = 404;
            }
            Response.StatusCode = 200;
            return userEvent;
        }

        /// <summary>
        /// Create a user event
        /// </summary>
        /// <param name="user"></param>
        /// <response code="200">Success</response>
        /// <response code="409">This event already exists</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserEvent), 200)]
        [ProducesResponseType(typeof(UserEvent), 409)]
        public void Post([FromBody] UserEvent userEvent)
        {
            List<UserEvent> userEvents = (List<UserEvent>)_userEventRepository.Get();
            if (userEvents.Contains(userEvent))
            {
                Response.StatusCode = 409;
                return;
            }
            _userEventRepository.Add(userEvent);
            Response.StatusCode = 200;
        }

        /// <summary>
        /// Update a user event by unique id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        /// <response code="404">A user event with this ID was not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserEvent), 200)]
        [ProducesResponseType(typeof(UserEvent), 404)]
        [ProducesResponseType(typeof(UserEvent), 409)]
        public void Put(int id, [FromBody] UserEvent userEvent)
        {
            _userEventRepository.Update(id, userEvent);
            UserEvent uEvent = _userEventRepository.Get(id);
            if (uEvent == null)
            {
                Response.StatusCode = 404;
                return;
            }
            List<UserEvent> userEvents = (List<UserEvent>)_userEventRepository.Get();
            if (userEvents.Where(uEvnt => uEvnt.Equals(userEvent)).Count() > 1)
            {
                Response.StatusCode = 409;
                return;
            }
            _userEventRepository.Update(id, userEvent);
            Response.StatusCode = 200;
        }

        /// <summary>
        /// Deleting a user event by unique id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        /// <response code="404">A user event with this ID was not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UserEvent), 200)]
        [ProducesResponseType(typeof(UserEvent), 404)]
        public void Delete(int id)
        {
           
            if (_userEventRepository.Get(id) == null)
            {
                Response.StatusCode = 404;
                return;
            }
            _userEventRepository.Delete(id);
            Response.StatusCode = 200;
        }

        /// <summary>
        /// Deleting all user events
        /// </summary>
        /// <returns>Users</returns>
        /// <response code="200">Success</response>
        /// <response code="404">User events not found</response>
        [HttpDelete]
        [ProducesResponseType(typeof(List<UserEvent>), 200)]
        [ProducesResponseType(typeof(List<UserEvent>), 404)]
        public void Delete()
        {
            if (_userEventRepository.Get() == null)
            {
                Response.StatusCode = 404;
                return;
            }
            _userEventRepository.DeleteAll();
            Response.StatusCode = 200;
        }
    }
}
