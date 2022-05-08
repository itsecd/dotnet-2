using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Server.Model;
using Server.Repositories;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJSONUserRepository _userRepositiry;
        //private readonly List<Task> tasks;
        public UserController(IJSONUserRepository userRepository) => _userRepositiry = userRepository;

        /// <summary>
        /// Getting all users
        /// </summary>
        /// <returns>Users</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Users not found</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<User>), 200)]
        [ProducesResponseType(typeof(List<User>), 404)]
        public IEnumerable<User> Get()
        {
            List<User> users = (List<User>)_userRepositiry.GetUsers();
            if(users == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Response.StatusCode=200;
            return users;
        }

        /// <summary>
        /// Getting a user by unique id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User</returns>
        /// <response code="200">Success</response>
        /// <response code="404">A user event with this ID was not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(User), 404)]
        public User Get(int id)
        {
            User user = _userRepositiry.GetUser(id);
            if(user == null)
            {
                Response.StatusCode = 404;
            }
            Response.StatusCode = 200;
            return user;
        }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="user"></param>
        /// <response code="200">Success</response>
        /// <response code="409">A user with this name already exists</response>
        [HttpPost]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(User), 409)]
        public void Post([FromBody] User user)
        {
            List<User> users = (List<User>)_userRepositiry.GetUsers();
            if(users.Exists(us => us.Name == user.Name))
            {
                Response.StatusCode = 409;
                return;
            }
            _userRepositiry.AddUser(user);
            Response.StatusCode = 200;
        }

        /// <summary>
        /// Update a user by unique id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        /// <response code="404">A user with this ID was not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(User), 404)]
        [ProducesResponseType(typeof(User), 409)]
        public void Put(int id, [FromBody] User user)
        {
            User us = _userRepositiry.GetUser(id);
            if (us == null)
            {
                Response.StatusCode = 404;
                return;
            }
            List<User> users = (List<User>)_userRepositiry.GetUsers();
            if(users.Exists(u => u.Name == user.Name && u.Id != id))
            {
                Response.StatusCode = 409;
                return;
            }
            _userRepositiry.UpdateUser(id, user);
            Response.StatusCode = 200;
        }

        /// <summary>
        /// Deleting a user by unique id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        /// <response code="404">A user with this ID was not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(User), 404)]
        public void Delete(int id)
        {
            if(_userRepositiry.GetUser(id) == null)
            {
                Response.StatusCode = 404;
                return;
            }
            _userRepositiry.DeleteUsers(id);
            Response.StatusCode = 200;
        }

        /// <summary>
        /// Deleting all users
        /// </summary>
        /// <returns>Users</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Users not found</response>
        [HttpDelete]
        [ProducesResponseType(typeof(List<User>), 200)]
        [ProducesResponseType(typeof(List<User>), 404)]
        public void Delete()
        {
            if(_userRepositiry.GetUsers() == null)
            {
                Response.StatusCode = 404;
                return;
            }
            _userRepositiry.DeleteAllUsers();
            Response.StatusCode = 200;
        }
    }
}
