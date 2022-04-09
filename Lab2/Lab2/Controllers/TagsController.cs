using Microsoft.AspNetCore.Mvc;
using Lab2.Models;
using Lab2.Repositories;
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

        // GET: api/<TagsController>
        [HttpGet]
        public IEnumerable<Tags> Get()
        {
            return _tagRepository.GetTags();
        }
        // GET api/<TagsController>/5
        [HttpGet("{id:int}")]
        public Tags Get(int id)
        {
            return _tagRepository.GetTags().Where(tags => tags.TagId == id).Single();
        }


        // POST api/<TagsController>
        [HttpPost]
        public void Post([FromBody] Tags value)
        {
            _tagRepository.AddTag(value);
        }

        // PUT api/<TagsController>/5
        [HttpPut("{id:int}")]
        public void Put(int id, [FromBody] Tags value)
        {
            var tagsIndex = _tagRepository.GetTags().FindIndex(tags => tags.TagId == id);

            if (tagsIndex > 0)
            {
                _tagRepository.GetTags()[tagsIndex] = value;
            }
        }

        // DELETE api/<TagsController>/5
        [HttpDelete]
        public void Delete()
        {
            _tagRepository.RemoveAllTags();
        }
        // DELETE api/<TaskListController>/5
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _tagRepository.RemoveTag(id);
        }
    }
}
