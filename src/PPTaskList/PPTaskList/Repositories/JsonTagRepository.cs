using Microsoft.Extensions.Configuration;
using PPTask.Controllers.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

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
        /// Получение файла хранения
        /// </summary>
        public JsonTagRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("TagsFile");
        }

        /// <summary>
        /// Список тегов
        /// </summary>
        private List<Tag> _tags;

        /// <summary>
        /// Асинхронный метод чтения из файла
        /// </summary>
        /// <returns> Task </returns>
        private async System.Threading.Tasks.Task ReadFromFileAsync()
        {
            if (_tags != null) return;

            if (!File.Exists(_storageFileName))
            {
                _tags = new List<Tag>();
                return;
            }

            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _tags = await JsonSerializer.DeserializeAsync<List<Tag>>(fileReader);
        }

        /// <summary>
        /// Асинхронный метод записи в файл
        /// </summary>
        /// <returns> Task </returns>
        private async System.Threading.Tasks.Task WriteToFileAsync()
        {
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            await JsonSerializer.SerializeAsync<List<Tag>>(fileWriter, _tags);
        }

        /// <summary>
        /// Асинхронный метод добавления тега 
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <returns> Task </returns>
        public async System.Threading.Tasks.Task AddTag(Tag tag)
        {
            await ReadFromFileAsync();
            _tags.Add(tag);
            await WriteToFileAsync();
        }

        /// <summary>
        /// Асинхронный метод удаления тега  
        /// </summary>
        /// <param name="id">Идентификатор тега</param>
        ///  <returns> Task </returns>
        public async System.Threading.Tasks.Task RemoveTag(int id)
        {
            if (_tags != null)
            {
                await ReadFromFileAsync();
                _tags.RemoveAll(tag => tag.TagId == id);
                await WriteToFileAsync();
            }
        }

        /// <summary>
        /// Метод получения всех тегов 
        /// </summary>
        /// <returns>Список тегов</returns>
        public async System.Threading.Tasks.Task<List<Tag>> GetTags()
        {
            await ReadFromFileAsync();
            return _tags;
        }
    }
}
