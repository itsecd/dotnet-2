using Microsoft.Extensions.Configuration;
using PPTask.Controllers.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace PPTask.Repositories
{
    public class JsonTagRepository : ITagRepository
    {
        private readonly string _storageFileName;

        public JsonTagRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("TagsFile");
        }

        private List<Tags> _tags;

        private async System.Threading.Tasks.Task ReadFromFileAsync()
        {
            if (_tags != null) return;

            if (!File.Exists(_storageFileName))
            {
                _tags = new List<Tags>();
                return;
            }

            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _tags = await JsonSerializer.DeserializeAsync<List<Tags>>(fileReader);
        }

        private async System.Threading.Tasks.Task WriteToFileAsync()
        {
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            await JsonSerializer.SerializeAsync<List<Tags>>(fileWriter, _tags);
        }

        public async System.Threading.Tasks.Task AddTag(Tags tag)
        {
            await ReadFromFileAsync();
            _tags.Add(tag);
            await WriteToFileAsync();
        }

        public async System.Threading.Tasks.Task RemoveAllTags()
        {
            await ReadFromFileAsync();
            _tags.RemoveRange(0, _tags.Count);
            await WriteToFileAsync();

        }

        public async System.Threading.Tasks.Task RemoveTag(int id)
        {
            if(id < _tags.Count)
            {
                await ReadFromFileAsync();
                _tags.RemoveAt(id);
                await WriteToFileAsync();
            }
        }

        public async System.Threading.Tasks.Task<List<Tags>> GetTags()
        {
            await ReadFromFileAsync();
            return _tags;
        }
    }
}
