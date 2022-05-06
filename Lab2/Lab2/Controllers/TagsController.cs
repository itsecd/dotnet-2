using Lab2.Dto;
using Lab2.Models;
using Lab2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Lab2.Controllers
{
    /// <summary>
    /// Контроллер для тегов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        /// <summary>
        /// Репозиторий тегов
        /// </summary>
        private readonly ITagRepository _tagRepository;

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        public TagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        /// <summary>
        /// Получение всех тэгов
        /// </summary>
        /// <returns>All Tags</returns>
        [HttpGet]
        public ActionResult<List<Tags>> Get() => _tagRepository.GetTags();

        /// <summary>
        /// Получение тэга по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Tag</returns>
        [HttpGet("{id:int}")]
        public ActionResult<Tags> Get(int id)
        {
            try
            {
                if (id < -1)
                {
                    return NotFound();
                }
                var tag = _tagRepository.Get(id);
                return tag;
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Добавление тэга
        /// </summary>
        /// <param name="tag">Новый тэг</param>
        [HttpPost]
        public IActionResult Post([FromBody] TagDto tag)
        {

            try
            {
                _tagRepository.AddTag(new Tags { Name = tag.Name, Color = tag.Color });
                return CreatedAtAction(nameof(Post), tag);
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Изменение параметров тэга по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="tag">Новый тэг</param>
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] TagDto tag)
        {

            try
            {
                _tagRepository.UpdateTag(id, new Tags { TagId = id, Name = tag.Name, Color = tag.Color });
                return Ok();
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
        /// Удаление всех тэгов
        /// </summary>
        [HttpDelete]
        public IActionResult Delete()
        {
            try
            {
                _tagRepository.RemoveAllTags();
                return Ok();
            }
            catch
            {
                return Problem();
            }
        }

        /// <summary>
        /// Удаление тэга по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _tagRepository.RemoveTag(id);
                return Ok();
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
