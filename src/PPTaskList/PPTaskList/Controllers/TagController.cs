using Microsoft.AspNetCore.Mvc;
using PPTask.Controllers.Model;
using PPTask.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PPTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public IEnumerable<Tags> Get()
        {
            return (IEnumerable<Tags>)_tagRepository.GetTags().Result;
        }

        [HttpGet("{id}")]
        public Tags Get(int id)
        {
            if (id > 0 && _tagRepository.GetTags().Result[id] != null)
            {
                return _tagRepository.GetTags().Result[id];
            }
            else
                throw new IndexOutOfRangeException();
        }

        [HttpPost]
        public void Post([FromBody] Tags value)
        {
            _tagRepository.AddTag(value);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Tags value)
        {
            if (id > 0 && _tagRepository.GetTags().Result[id] != null)
            {
                _tagRepository.GetTags().Result[id] = value;
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _tagRepository.RemoveAllTags();
        }
    }
}
