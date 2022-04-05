using PPTaskList.Controllers.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PPTaskList.Repositories
{
    public class JsonExecutorRepository: IExecutorRepository
    {
        private const string StorageFileName = "executors.json";

        private List<Executor> _executors;

        private async void ReadFromFileAsync()
        {
            if (_executors != null) return;

            if (!File.Exists(StorageFileName))
            {
                _executors = new List<Executor>();
                return;
            }

            using var fileReader = new FileStream(StorageFileName, FileMode.Open);
            _executors = await JsonSerializer.DeserializeAsync<List<Executor>>(fileReader);
        }

        private async void WriteToFileAsync()
        {
            using var fileWriter = new FileStream(StorageFileName, FileMode.Create);
            await JsonSerializer.SerializeAsync<List<Executor>>(fileWriter, _executors);
        }

        public void AddExecutor(Executor executor)
        {
            ReadFromFileAsync();
            _executors.Add(executor);
            WriteToFileAsync();
        }

        public void RemoveAllExecutors()
        {
            ReadFromFileAsync();
            _executors.RemoveRange(0, _executors.Count);
            WriteToFileAsync();

        }

        public List<Executor> GetExecutors()
        {
            ReadFromFileAsync();
            return _executors;
        }
    }
}
