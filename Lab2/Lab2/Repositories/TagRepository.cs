using Lab2.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Lab2.Repositories
{
    public class TagRepository:ITagRepository
    {
        private readonly string _storageFileName;
        public TagRepository() { }
        public TagRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("TagsFile");
        }
        private List<Tags> _tags;

        private async Task ReadFromFile()
        {
            if (_tags != null) return;

            if (!File.Exists(_storageFileName))
            {
                _tags = new List<Tags>();
                return;
            }
            await DeserializeFile();   
        }
        private async Task DeserializeFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Tags>));
            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _tags = (List<Tags>)xmlSerializer.Deserialize(fileReader);
        }
        private async Task WriteToFile()
        {
           await SerializeFile();
        }
        private async Task SerializeFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Tags>));
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _tags);
        }
        public async Task<int> AddTag(Tags tags)
        {
            await ReadFromFile();
            _tags.Add(tags);
            await WriteToFile();
            return tags.TagId;
        }

        public async Task RemoveAllTags()
        {
            await ReadFromFile();
            _tags.RemoveRange(0, _tags.Count);
            await WriteToFile();

        }
        public async Task SaveFile()
        {
            await WriteToFile();
        }
        public async Task<List<Tags>> GetTags()
        {
            await ReadFromFile();
            return _tags;
        }
        public async Task<int> RemoveTag(int id)
        {
            await ReadFromFile();
            _tags.RemoveAt(id);
            await WriteToFile();
            return id;
        }

    }
}
