using Lab2.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;


namespace Lab2.Repositories
{
    public class ExecutorRepository : IExecutorRepository
    {

        private readonly string _storageFileName = "executors.xml";
        private List<Executor> _executors;

        public ExecutorRepository(IConfiguration configuration = null)
        {
            if (configuration is not null)
            {
                _storageFileName = configuration.GetValue<string>("ExecutorsFile");
            }

            if (!File.Exists(_storageFileName))
            {
                _executors = new List<Executor>();
                return;
            }
            var xmlSerializer = new XmlSerializer(typeof(List<Executor>));
            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _executors = (List<Executor>)xmlSerializer.Deserialize(fileReader);
        }

        
        public void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Executor>));
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _executors);
        }

        public int AddExecutor(Executor executor)
        {
            var maxId = _executors.Max(ex => ex.ExecutorId);
            executor.ExecutorId = maxId + 1;
            _executors.Add(executor);
            return executor.ExecutorId;
        }

        public void RemoveAllExecutors()
        {
            _executors.RemoveRange(0, _executors.Count);
        }

        public List<Executor> GetExecutors()
        {
            return _executors;
        }
        public int RemoveExecutor(int id)
        {
            _executors.RemoveAt(id - 1);
            return id;
        }
        public int UpdateExecutor(int id, Executor newExecutor)
        {
            var executorIndex = _executors.FindIndex(p => p.ExecutorId == id);
            _executors[executorIndex] = newExecutor;
            return id;
        }

    }
}
