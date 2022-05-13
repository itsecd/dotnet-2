using Microsoft.AspNetCore.Mvc;
using System;
using Lab2Server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab2Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id) //получение user
        {
            try
            {
                return _userRepository.FindUser(id);
            }
            catch (UserRepositoryException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // POST api/<UserController>
        [HttpPost("{id}/reminders")]
        public ActionResult Post(int id, Reminder newReminder) //добавление для reminder
        {
            try
            {
                _userRepository.AddReminder(id, newReminder);
                return Ok();
            }
            catch (UserRepositoryException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, string newName) // изменение для user
        {
            try
            {
                _userRepository.ChangeName(id, newName);
                return Ok();
            }
            catch (UserRepositoryException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return Problem();
            }
        }
        [HttpPut("{userid}/reminders/{id}")]
        public ActionResult Put(int userid, int id, Reminder reminder) // изменение для reminder
        {
            try
            {
                _userRepository.ChangeReminder(userid, id, reminder);
                return Ok();
            }
            catch (UserRepositoryException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return Problem();
            }
        }
        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) // удаление User
        {
            try
            {
                _userRepository.RemoveUser(id);
                return Ok();
            }
            catch (Exception)
            {
                return Problem();
            }
        }
        [HttpDelete("{userId}/reminders/{id}")]
        public ActionResult Delete(int userId, int id) // удаление Reminder
        {
            try
            {
                _userRepository.RemoveReminder(userId, id);
                return Ok();
            }
            catch (UserRepositoryException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return Problem();
            }
        }
    }
}
