using Lab2.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Lab2.Repositories
{
    /// <summary>
    /// Класс для тэга с добавлением, удалением, изменением, сериализацией и десериализацией исполнителей в формате xml 
    /// </summary>
    public class TagRepository : ITagRepository
    {
        /// <summary>
        /// Название файла хранения
        /// </summary>
        private readonly string _storageFileName = "tag.xml";

        /// <summary>
        /// Список тэгов
        /// </summary>
        private readonly List<Tags> _tags;

        /// <summary>
        /// Файл хранения
        /// </summary>
        public TagRepository(IConfiguration configuration = null)
        {
            if (configuration != null)
            {
                _storageFileName = configuration.GetValue<string>("TagsFile");
            }

            if (!File.Exists(_storageFileName))
            {
                _tags = new List<Tags>();
                return;
            }
            var xmlSerializer = new XmlSerializer(typeof(List<Tags>));
            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _tags = (List<Tags>)xmlSerializer.Deserialize(fileReader);
        }

        /// <summary>
        /// Мeтод получения тэга по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор тэга</param>
        /// <returns>Тэг</returns>
        public Tags Get(int id) => _tags.FirstOrDefault(p => p.TagId == id);

        /// <summary>
        /// Метод записи в файл
        /// </summary>
        public void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Tags>));
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _tags);
        }

        /// <summary>
        /// Мeтод добавления тэга 
        /// </summary>
        /// <param name="tag">Тэг</param>
        ///<returns>Идентификатор тэг</returns>
        public int AddTag(Tags tag)
        {
            if (tag.TagId == default)
            {
                if (_tags.Count == 0)
                {
                    tag.TagId = 1;
                    _tags.Add(tag);
                }
                else
                {
                    tag.TagId = _tags.Max(t => t.TagId) + 1;
                    _tags.Add(tag);
                }
            }
            return tag.TagId;
        }

        /// <summary>
        /// Мeтод удаления всех тэгов
        /// </summary>
        public void RemoveAllTags()
        {
            _tags.RemoveRange(0, _tags.Count);
        }

        /// <summary>
        /// Мeтод получения всех тэгов 
        /// </summary>
        /// <returns>Список тэгов</returns>
        public List<Tags> GetTags()
        {
            return _tags;
        }

        /// <summary>
        /// Мeтод удаления тэга по идентификатору
        /// </summary>
        public int RemoveTag(int id)
        {
            var DeletedTag = Get(id);
            _tags.Remove(DeletedTag);
            return id;
        }

        /// <summary>
        /// Мeтод изменения тэга по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор тэг</param>
        /// <param name="newTag">Измененный тэг</param>
        public int UpdateTag(int id, Tags newTag)
        {
            var tagIndex = _tags.FindIndex(p => p.TagId == id);
            _tags[tagIndex] = newTag;
            return id;
        }
    }
}
