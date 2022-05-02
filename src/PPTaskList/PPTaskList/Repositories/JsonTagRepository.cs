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

        private List<Tag> _tags;

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

        private async System.Threading.Tasks.Task WriteToFileAsync()
        {
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            await JsonSerializer.SerializeAsync<List<Tag>>(fileWriter, _tags);
        }

        public async System.Threading.Tasks.Task AddTag(Tag tag)
        {
            await ReadFromFileAsync();
            _tags.Add(tag);
            await WriteToFileAsync();
        }

        public async System.Threading.Tasks.Task RemoveTag(int id)
        {
            if (_tags != null)
            {
                await ReadFromFileAsync();
                _tags.RemoveAll(tag => tag.TagId == id);
                await WriteToFileAsync();
            }
        }

        public async System.Threading.Tasks.Task<List<Tag>> GetTags()
        {
            await ReadFromFileAsync();
            return _tags;
        }
    }
}
