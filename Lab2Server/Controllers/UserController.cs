using Microsoft.AspNetCore.Mvc;
using System;
using Lab2Server.Models;

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

        /// <summary>
        /// Контроллер принимает идентификатор пользователя
        /// Возвращает пользователя с соответствующим идентификатором
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
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

        /// <summary>
        /// Контроллер принимает идентификатор пользователя и заметку, которую требуется добавить
        /// Данный контроллер добавляет заметку в список заметок пользователя
        /// </summary>
        [HttpPost("{id}/reminders")]
        public ActionResult Post(int id, Reminder newReminder)
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

        /// <summary>
        /// Контроллер принимает идентификатор и новое имя пользователя
        /// Данный контроллер изменяет имя пользователя с соответствующим идентификатором
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult Put(int id, string newName)
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

        /// <summary>
        /// Контроллер принимает идентификатор пользователя, идентификатор заметки и заметку
        /// Данный контроллер изменяет заметку с заданным идентификатором заметки в списке заметок пользователя с заданным идентификатором пользователя на переданную заметку
        /// </summary>
        [HttpPut("{userid}/reminders/{id}")]
        public ActionResult Put(int userid, int id, Reminder reminder)
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

        /// <summary>
        /// Контроллер принимает идентификатор пользователя
        /// Данный контроллер удаляет пользователя с соответствующим идентификатором
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
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

        /// <summary>
        /// Контроллер принимает идентификатор пользователя и идентификатор заметки
        /// Данный контроллер удаляет заметку с заданным идентификатором заметки в списке заметок пользователя с заданным идентификатором пользователя
        /// </summary>
        [HttpDelete("{userId}/reminders/{id}")]
        public ActionResult Delete(int userId, int id)
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
