using Microsoft.Extensions.Configuration;
using PPTask.Controllers.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace PPTask.Repositories
{
    public class JsonExecutorRepository: IExecutorRepository
    {
        private readonly string _storageFileName;

        public JsonExecutorRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("ExecutorsFile");
        }

        private List<Executor> _executors;

        private async System.Threading.Tasks.Task ReadFromFileAsync()
        {
            if (_executors != null) return;

            if (!File.Exists(_storageFileName))
            {
                _executors = new List<Executor>();
                return;
            }

            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _executors = await JsonSerializer.DeserializeAsync<List<Executor>>(fileReader);
        }

        private async System.Threading.Tasks.Task WriteToFileAsync()
        {
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            await JsonSerializer.SerializeAsync<List<Executor>>(fileWriter, _executors);
        }

        public async System.Threading.Tasks.Task AddExecutor(Executor executor)
        {
            await ReadFromFileAsync();
            _executors.Add(executor);
            await WriteToFileAsync();
        }

        public async System.Threading.Tasks.Task RemoveExecutor(int id)
        {
            if (id < _executors.Count)
            {
                await ReadFromFileAsync();
                _executors.RemoveAt(id);
                await WriteToFileAsync();
            }
        }

        public async System.Threading.Tasks.Task RemoveAllExecutors()
        {
            await ReadFromFileAsync();
            _executors.RemoveRange(0, _executors.Count);
            await WriteToFileAsync();
        }

        public async System.Threading.Tasks.Task<List<Executor>> GetExecutors()
        {
            await ReadFromFileAsync();
            return _executors;
        }
    }
}
