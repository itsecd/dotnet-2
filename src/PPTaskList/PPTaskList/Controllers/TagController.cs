using Microsoft.AspNetCore.Mvc;
using PPTask.Dto;
using PPTask.Model;
using PPTask.Repositories;
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
                return _tagRepository.GetTags();
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
        [HttpGet("{id:int}")]
        public ActionResult<Tag> Get(int id)
        {
            try
            {
                if (id < -1)
                {
                    return NotFound();
                }

                return _tagRepository.GetTags().Single(tag => tag.TagId == id);
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
        public ActionResult Post([FromBody] TagDto value)
        {
            try
            {
                _tagRepository.AddTag(new Tag { TagColour = value.TagColour, TagStatus = value.TagStatus });
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
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] TagDto value)
        {
            try
            {
                var tagIndex = _tagRepository.GetTags().FindIndex(tag => tag.TagId == id);
                if (tagIndex < -1 || id < -1)
                {
                    return NotFound();
                }

                _tagRepository.GetTags()[tagIndex] = new Tag { TagColour = value.TagColour, TagStatus = value.TagStatus };
                return Ok();

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
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var tagIndex = _tagRepository.GetTags().FindIndex(tag => tag.TagId == id);
                if (tagIndex < -1)
                {
                    return NotFound();
                }

                _tagRepository.RemoveTag(id);
                return Ok();
            }
            catch
            {
                return Problem();
            }
        }
    }
}
