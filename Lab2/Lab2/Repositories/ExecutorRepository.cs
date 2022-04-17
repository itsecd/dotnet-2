using Lab2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Repositories
{
    public class ExecutorRepository: IExecutorRepository
    {
        private const string StorageFileName = "executors.xml";

        private List<Executor> _executors;
        private async Task ReadFromFile()
        {
            if (_executors != null) return;

            if (!File.Exists(StorageFileName))
            {
                _executors = new List<Executor>();
                return;
            }
            await DeserializeFile();
            
        }
        private async Task DeserializeFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Executor>));
            using var fileReader = new FileStream(StorageFileName, FileMode.Open);
            _executors = (List<Executor>)xmlSerializer.Deserialize(fileReader);
        }
        private async Task SerializeFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Executor>));
            using var fileWriter = new FileStream(StorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _executors);
        }
        private async Task WriteToFile()
        {
            await SerializeFile();
        }

        public int AddExecutor(Executor executor)
        {
            ReadFromFile();
            _executors.Add(executor);
            WriteToFile();
            return executor.ExecutorId;
        }
        public void SaveFile()
        {
            WriteToFile();
        }
        public void RemoveAllExecutors()
        {
            ReadFromFile();
            _executors.RemoveRange(0, _executors.Count);
            WriteToFile();
        }

        public List<Executor> GetExecutors()
        {
            ReadFromFile();
            return _executors;
        }
        public int RemoveExecutor(int id)
        {
            ReadFromFile();
            _executors.RemoveAt(id-1);
            WriteToFile();
            return id;
        }


    }
}
