using Lab2.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Lab2.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly string _storageFileName = "tag.xml";
        private List<Tags> _tags;
        public TagRepository(IConfiguration configuration = null)
        {
            if (configuration != null)
            {
                _storageFileName = configuration.GetValue<string>("TagsFile");
            }
            if (_tags != null)
            {
                return;
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


        public void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Tags>));
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _tags);
        }


        public int AddTag(Tags tags)
        {
            var maxId = _tags.Max(t => t.TagId);
            tags.TagId = maxId + 1;
            _tags.Add(tags);
            return tags.TagId;
        }

        public void RemoveAllTags()
        {
            _tags.RemoveRange(0, _tags.Count);
        }

        public List<Tags> GetTags()
        {
            return _tags;
        }
        public int RemoveTag(int id)
        {
            _tags.RemoveAt(id);
            return id;
        }
        public int UpdateTag(int id, Tags newTag)
        {
            var tagIndex = _tags.FindIndex(p => p.TagId == id);
            _tags[tagIndex] = newTag;
            return id;
        }

    }
}
