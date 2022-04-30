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

        private void ReadFromFile()
        {
            if (_tags != null) return;

            if (!File.Exists(_storageFileName))
            {
                _tags = new List<Tags>();
                return;
            }
            var xmlSerializer = new XmlSerializer(typeof(List<Tags>));
            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _tags = (List<Tags>)xmlSerializer.Deserialize(fileReader);
        }

        private void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Tags>));
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _tags);
        }


        public int AddTag(Tags tags)
        {
             ReadFromFile();
            _tags.Add(tags);
             WriteToFile();
            return tags.TagId;
        }

        public void RemoveAllTags()
        {
            ReadFromFile();
            _tags.RemoveRange(0, _tags.Count);
            WriteToFile();

        }
     
        public List<Tags> GetTags()
        {
            ReadFromFile();
            return _tags;
        }
        public int RemoveTag(int id)
        {
            ReadFromFile();
            _tags.RemoveAt(id);
            WriteToFile();
            return id;
        }
        public int UpdateTag(int id, Tags newTag)
        {
            ReadFromFile();
            var tagIndex = _tags.FindIndex(p => p.TagId == id);
            _tags[tagIndex] = newTag;
            WriteToFile();
            return id;
        }

    }
}
