using Microsoft.Extensions.Configuration;
using PPTask.Controllers.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace PPTask.Repositories
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly string _storageFileName;

        public JsonTaskRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("TasksFile");
        }

        private List<Task> _tasks;

        private async System.Threading.Tasks.Task ReadFromFileAsync()
        {
            if (_tasks != null) return;

            if (!File.Exists(_storageFileName))
            {
                _tasks = new List<Task>();
                return;
            }

            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _tasks = await JsonSerializer.DeserializeAsync<List<Task>>(fileReader);
        }

        private async System.Threading.Tasks.Task WriteToFileAsync()
        {
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            await JsonSerializer.SerializeAsync<List<Task>>(fileWriter, _tasks);
        }

        public async System.Threading.Tasks.Task AddTask(Task task)
        {
            await ReadFromFileAsync();
            _tasks.Add(task);
            await WriteToFileAsync();
        }

        public async System.Threading.Tasks.Task RemoveAllTasks()
        {
            await ReadFromFileAsync();
            _tasks.RemoveRange(0, _tasks.Count);
            await WriteToFileAsync();

        }

        public async System.Threading.Tasks.Task<List<Task>> GetTasks()
        {
            await ReadFromFileAsync();
            return _tasks;
        }
    }
}
