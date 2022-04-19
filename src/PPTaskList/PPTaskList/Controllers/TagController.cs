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
        public IEnumerable<Tags> Get()
        {
            return (IEnumerable<Tags>)_tagRepository.GetTags().Result;
        }

        /// <summary>
        /// Метод получения тега по идентификатору 
        /// </summary>
        /// <param name="id">Идентификатор тега</param>
        /// <returns>Тег</returns>
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

        /// <summary>
        /// Метод добавления тега 
        /// </summary>
        /// <param name="value">Новый тег</param>
        [HttpPost]
        public void Post([FromBody] Tags value)
        {
            _tagRepository.AddTag(value);
        }

        /// <summary>
        /// Метод замены тега 
        /// </summary>
        /// <param name="value">Новый тег</param>
        /// /// <param name="id">Идентификатор заменяемого тега</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Tags value)
        {
            if (id > 0 && _tagRepository.GetTags().Result[id] != null)
            {
                _tagRepository.GetTags().Result[id] = value;
            }
        }

        /// <summary>
        /// Метод удаления тега 
        /// </summary>
        /// <param name="id">Идентификатор удаляемого тега</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _tagRepository.GetTags().Result.RemoveAt(id);
            //_tagRepository.RemoveAllTags();
        }
    }
}
