using Microsoft.AspNetCore.Mvc;
using PPTask.Controllers.Model;
using PPTask.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PPTask.Controllers
{
    /// <summary>
    /// Контроллер тегов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        /// <summary>
        /// Репозиторий тегов
        /// </summary>
        private readonly ITagRepository _tagRepository;

        /// <summary>
        /// Конструктор с параметрами. В качестве параметра принимает репозиторий.
        /// </summary>
        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        /// <summary>
        /// Метод получения тегов
        /// </summary>
        /// <returns>Теги</returns>
        [HttpGet]
        public ActionResult<List<Tag>> Get()
        {
            try
            {
                return _tagRepository.GetTags().Result;
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Метод получения тега по идентификатору 
        /// </summary>
        /// <param name="id">Идентификатор тега</param>
        /// <returns>Тег</returns>
        [HttpGet("{id}")]
        public ActionResult <Tag> Get(int id)
        {
            try
            {
                return _tagRepository.GetTags().Result.Where(tag => tag.TagId == id).Single();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Метод добавления тега 
        /// </summary>
        /// <param name="value">Новый тег</param>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Tag value)
        {
           try
           {
                _tagRepository.AddTag(value);
                return Ok();
           }
           catch
           {
                return Problem();
           }
        }

        /// <summary>
        /// Метод замены тега 
        /// </summary>
        /// <param name="value">Новый тег</param>
        /// /// <param name="id">Идентификатор заменяемого тега</param>
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Tag value)
        {
            try
            {
                var tagIndex = _tagRepository.GetTags().Result.FindIndex(tag => tag.TagId == id);
                _tagRepository.GetTags().Result[tagIndex] = value;
                return Ok();

            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Метод удаления тега 
        /// </summary>
        /// <param name="id">Идентификатор удаляемого тега</param>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _tagRepository.RemoveTag(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return Problem();
            }
        }
    }
}
