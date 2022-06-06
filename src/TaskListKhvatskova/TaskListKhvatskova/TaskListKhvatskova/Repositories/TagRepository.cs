using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TaskListKhvatskova.Models;

namespace TaskListKhvatskova.Repositories
{
    /// <summary>
    /// Класс для тэга с добавлением, удалением, изменением, сериализацией и десериализацией исполнителей в формате xml 
    /// </summary>
    public class TagRepository : ITagRepository
    {
        /// <summary>
        /// Название файла хранения
        /// </summary>
        private readonly string _storageFileName = "tag.json";

        /// <summary>
        /// Список тэгов
        /// </summary>
        private readonly List<Tags> _tags;

        public TagRepository()
        {
            _tags = new();
        }

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
            string jsonString = File.ReadAllText(_storageFileName);
            _tags = JsonSerializer.Deserialize<List<Tags>>(jsonString);
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
            string jsonString = JsonSerializer.Serialize(_tags);
            File.WriteAllText(_storageFileName, jsonString);
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
            else
            {
                if (_tags.FindIndex(t => t.TagId == tag.TagId) == -1)
                {
                    _tags.Add(tag);
                }
                else
                {
                    throw new Exception("This ID already exists");
                }
            }
            WriteToFile();
            return tag.TagId;
        }

        /// <summary>
        /// Мeтод удаления всех тэгов
        /// </summary>
        public void RemoveAllTags()
        {
            _tags.RemoveRange(0, _tags.Count);
            WriteToFile();
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
            var deletedTag = Get(id);
            _tags.Remove(deletedTag);
            WriteToFile();
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
            WriteToFile();
            return id;
        }
    }
}
