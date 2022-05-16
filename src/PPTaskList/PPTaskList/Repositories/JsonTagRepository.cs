using Microsoft.Extensions.Configuration;
using PPTask.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace PPTask.Repositories
{
    /// <summary>
    /// Класс сериализации и десериализации тегов в формате json 
    /// </summary>
    public class JsonTagRepository : ITagRepository
    {
        /// <summary>
        /// Название файла хранения
        /// </summary>
        private readonly string _storageFileName;

        /// <summary>
        /// Список тегов
        /// </summary>
        private readonly List<Tag> _tags;

        /// <summary>
        /// Получение файла хранения
        /// </summary>
        public JsonTagRepository(IConfiguration configuration = null)
        {
            if (configuration != null)
            {
                _storageFileName = configuration.GetValue<string>("TagsFile");
            }

            if (!File.Exists(_storageFileName))
            {
                _tags = new List<Tag>();
                return;
            }

            var repositoryJson = File.ReadAllText(_storageFileName);
            _tags =  JsonSerializer.Deserialize<List<Tag>>(repositoryJson);
        }

        /// <summary>
        /// Асинхронный метод записи в файл
        /// </summary>
        /// <returns> Task </returns>
        public async System.Threading.Tasks.Task WriteToFileAsync()
        {
            await using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            await JsonSerializer.SerializeAsync<List<Tag>>(fileWriter, _tags);
        }

        /// <summary>
        /// Метод добавления тега 
        /// </summary>
        /// <param name="tag">Тег</param>
        public void AddTag(Tag tag)
        {
            if (_tags.Count == 0)
            {
                tag.TagId = 1;
            }
            else
            {
                var maxId = _tags.Max(t => t.TagId);
                tag.TagId = maxId + 1;
            }
            _tags.Add(tag);
        }

        /// <summary>
        /// Метод удаления тега  
        /// </summary>
        /// <param name="id">Идентификатор тега</param>
        public void RemoveTag(int id)
        {
            if (_tags != null)
            {
                _tags.RemoveAll(tag => tag.TagId == id);
            }
        }

        /// <summary>
        /// Метод удаления всех тегов  
        /// </summary>
        public void RemoveAllTags()
        {
            if (_tags != null)
            {
                _tags.RemoveRange(0, _tags.Count);
            }
        }

        /// <summary>
        /// Метод получения всех тегов 
        /// </summary>
        /// <returns>Список тегов</returns>
        public List<Tag> GetTags()
        {
            return _tags;
        }
    }
}
