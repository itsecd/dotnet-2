using Lab2.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Lab2.Repositories
{
    public class ExecutorRepository: IExecutorRepository
    {

        private readonly string _storageFileName;
        public ExecutorRepository() { }
        public ExecutorRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("ExecutorsFile");
        }

        private List<Executor> _executors;

        private void ReadFromFile()
        {
            if (_executors != null) return;

            if (!File.Exists(_storageFileName))
            {
                _executors = new List<Executor>();
                return;
            }
            var xmlSerializer = new XmlSerializer(typeof(List<Executor>));
            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _executors = (List<Executor>)xmlSerializer.Deserialize(fileReader);
        }

        private void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Executor>));
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _executors);
        }

        public int AddExecutor(Executor executor)
        {
             ReadFromFile();
            _executors.Add(executor);
             WriteToFile();
            return executor.ExecutorId;
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
        public int UpdateExecutor(int id, Executor newExecutor)
        {
            ReadFromFile();
            var executorIndex = _executors.FindIndex(p => p.ExecutorId == id);
            _executors[executorIndex] = newExecutor;
            WriteToFile();
            return id; 
        }

    }
}
