using Microsoft.AspNetCore.Mvc;
using Server.Exceptions;
using Server.Model;
using Server.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEventController : ControllerBase
    {
        private readonly IJSONUserEventRepository _userEventRepository;

        public UserEventController(IJSONUserEventRepository userEventRepository) => _userEventRepository = userEventRepository;


        /// <summary>
        /// Getting all user events
        /// </summary>
        /// <returns>User events</returns>
        /// <response code="200">Success</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserEvent>), 200)]
        public IEnumerable<UserEvent> Get()
        {
            try
            {
                var userEvents = _userEventRepository.GetUserEvents();
                Response.StatusCode = 200;
                return userEvents;
            }
            catch (NotFoundException)
            {
                Response.StatusCode = 404;
                return null;
            }
        }

        /// <summary>
        /// Getting a user event by unique id
        /// </summary>
        /// <param name="id">User event ID</param>
        /// <returns>User event</returns>
        /// <response code="200">Success</response>
        /// <response code="404">A user event with this ID was not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserEvent), 200)]
        [ProducesResponseType(typeof(UserEvent), 404)]
        public UserEvent Get(int id)
        {
            try
            {
                var userEvent = _userEventRepository.GetUserEvent(id);
                Response.StatusCode = 200;
                return userEvent;
            }
            catch (NotFoundException)
            {
                Response.StatusCode = 404;
                return null;
            }
        }

        /// <summary>
        /// Create a user event
        /// </summary>
        /// <param name="userEvent">User event</param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid event data</response>
        /// <response code="409">This event already exists</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserEvent), 200)]
        [ProducesResponseType(typeof(UserEvent), 400)]
        [ProducesResponseType(typeof(UserEvent), 409)]
        public void Post([FromBody] UserEvent userEvent)
        {
            try
            {
                _userEventRepository.AddUserEvent(userEvent);
                Response.StatusCode = 200;
            }
            catch (NotFoundException)
            {
                Response.StatusCode = 404;
            }
            catch (AlreadyExistException)
            {
                Response.StatusCode = 409;
            }
            catch (InvalidUserEventData)
            {
                Response.StatusCode = 400;
            }
        }

        /// <summary>
        /// Update a user event by unique id
        /// </summary>
        /// <param name="id">User event ID</param>
        /// <param name="userEvent">User event</param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid event data</response>
        /// <response code="404">A user event with this ID was not found</response>
        /// <response code="409">This event already exist</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserEvent), 200)]
        [ProducesResponseType(typeof(UserEvent), 400)]
        [ProducesResponseType(typeof(UserEvent), 404)]
        [ProducesResponseType(typeof(UserEvent), 409)]
        public void Put(int id, [FromBody] UserEvent userEvent)
        {
            try
            {
                _userEventRepository.UpdateUserEvent(id, userEvent);
                Response.StatusCode = 200;
            }
            catch (NotFoundException)
            {
                Response.StatusCode = 404;
            }
            catch (AlreadyExistException)
            {
                Response.StatusCode = 409;
            }
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
            try           
            {
                _userEventRepository.DeleteUserEvent(id);
                Response.StatusCode = 200;
            }
            catch (NotFoundException)
            {
                Response.StatusCode = 404;
            }
        }

        /// <summary>
        /// Deleting all user events
        /// </summary>
        /// <response code="200">Success</response>
        [HttpDelete]
        [ProducesResponseType(typeof(List<UserEvent>), 200)]
        public void Delete()
        {
            try
            {
                _userEventRepository.DeleteAllUserEvents();
                Response.StatusCode = 200;
            }
            catch (NotFoundException)
            {
                Response.StatusCode = 404;
            }
            
        }
    }
}
