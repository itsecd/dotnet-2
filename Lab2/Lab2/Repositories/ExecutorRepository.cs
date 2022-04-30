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

        private string _storageFileName;
        public ExecutorRepository() { }
        public ExecutorRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("ExecutorsFile");
        }

        private List<Executor> _executors;
        private async Task ReadFromFile()
        {
            if (_executors != null) return;

            if (!File.Exists(_storageFileName))
            {
                _executors = new List<Executor>();
                return;
            }
            await DeserializeFile();
        }

        private async Task DeserializeFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Executor>));
            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _executors = (List<Executor>)xmlSerializer.Deserialize(fileReader);
        }
        private async Task SerializeFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Executor>));
            using FileStream fileWriter = new FileStream(_storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _executors);
        }

        private async Task WriteToFile()
        {
            await SerializeFile();
        }

        public async Task<int> AddExecutor(Executor executor)
        {
            await ReadFromFile();
            _executors.Add(executor);
            await WriteToFile();
            return executor.ExecutorId;
        }

        public async Task SaveFile()
        {
            await WriteToFile();
        }

        public async Task RemoveAllExecutors()
        {
            await ReadFromFile();
            _executors.RemoveRange(0, _executors.Count);
            await WriteToFile();
        }

        public async Task<List<Executor>> GetExecutors()
        {
            await ReadFromFile();
            return _executors;
        }
        public async Task<int> RemoveExecutor(int id)
        {
            await ReadFromFile();
            _executors.RemoveAt(id-1);
            await WriteToFile();
            return id;
        }


    }
}
