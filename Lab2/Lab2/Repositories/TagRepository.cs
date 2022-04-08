using Lab2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Repositories
{
    public class TagRepository:ITagRepository
    {
        private const string StorageFileName = "tag.xml";

        private List<Tags> _tags;

        private void ReadFromFile()
        {
            if (_tags != null) return;

            if (!File.Exists(StorageFileName))
            {
                _tags = new List<Tags>();
                return;
            }

            var xmlSerializer = new XmlSerializer(typeof(List<Tags>));
            using var fileReader = new FileStream(StorageFileName, FileMode.Open);
            _tags = (List<Tags>)xmlSerializer.Deserialize(fileReader);
        }
        private void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Tags>));
            using var fileWriter = new FileStream(StorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _tags);
        }

        public void AddTag(Tags tags)
        {
            ReadFromFile();
            _tags.Add(tags);
            WriteToFile();
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
    }
}
