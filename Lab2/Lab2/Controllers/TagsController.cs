using Microsoft.AspNetCore.Mvc;
using Lab2.Models;
using Lab2.Repositories;
using Lab2.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System;

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
                var tag = _tagRepository.GetTags().Single(tag => tag.TagId == id);
                return tag;
            }
            catch (NotFoundException)
            {
                return NotFound();
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
        public IActionResult Post([FromBody] Tags tag)
        {

            try
            {
                _tagRepository.AddTag(tag);
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
                var tagsIndex = _tagRepository.GetTags().FindIndex(tags => tags.TagId == id);
                _tagRepository.GetTags()[tagsIndex] = tag;
                _tagRepository.SaveFile();
                return Ok();
            }
            catch (NotFoundException)
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
            catch (NotFoundException)
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
