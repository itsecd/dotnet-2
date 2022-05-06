using Lab2.Models;
using Lab2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {

        private readonly ITagRepository _tagRepository;

        public TagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        /// <summary>
        /// Получение всех тэгов
        /// </summary>
        /// <returns>All Tags</returns>
        // GET: api/<TagsController>
        [HttpGet]
        public ActionResult<List<Tags>> Get() => _tagRepository.GetTags();

        /// <summary>
        /// Получение тэга по его индентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Tag</returns>
        // GET api/<TagsController>/5
        [HttpGet("{id:int}")]

        public ActionResult<Tags> Get(int id)
        {
            try
            {
                if (id < -1)
                {
                    return NotFound();
                }

                var tag = _tagRepository.GetTags().Single(tag => tag.TagId == id);
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
        // POST api/<TagsController>
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
        // PUT api/<TagsController>/5
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Tags tag)
        {

            try
            {
                _tagRepository.UpdateTag(id, tag);
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
        // DELETE api/<TagsController>/5
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
        // DELETE api/<TaskListController>/5
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
