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
        private void ReadFromFile()
        {
            if (_executors != null) return;

            if (!File.Exists(StorageFileName))
            {
                _executors = new List<Executor>();
                return;
            }

            var xmlSerializer = new XmlSerializer(typeof(List<Executor>));
            using var fileReader = new FileStream(StorageFileName, FileMode.Open);
            _executors =  (List<Executor>)xmlSerializer.Deserialize(fileReader);
        }

        private void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Executor>));
            using var fileWriter = new FileStream(StorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _executors);
        }

        public void AddExecutor(Executor executor)
        {
            ReadFromFile();
            _executors.Add(executor);
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


    }
}
