using PPTaskList.Controllers.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PPTaskList.Repositories
{
    public class JsonTaskListRepository : ITaskListRepository
    {
        private const string StorageFileName = "tasks.json";

        private List<TaskList> _tasks;

        private async void ReadFromFileAsync()
        {
            if (_tasks != null) return;

            if (!File.Exists(StorageFileName))
            {
                _tasks = new List<TaskList>();
                return;
            }

            using var fileReader = new FileStream(StorageFileName, FileMode.Open);
            _tasks = await JsonSerializer.DeserializeAsync<List<TaskList>>(fileReader);
        }

        private async void WriteToFileAsync()
        {
            using var fileWriter = new FileStream(StorageFileName, FileMode.Create);
            await JsonSerializer.SerializeAsync<List<TaskList>>(fileWriter, _tasks);
        }

        public void AddTask(TaskList task)
        {
            ReadFromFileAsync();
            _tasks.Add(task);
            WriteToFileAsync();
        }

        public void RemoveAllTasks()
        {
            ReadFromFileAsync();
            _tasks.RemoveRange(0, _tasks.Count);
            WriteToFileAsync();

        }

        public List<TaskList> GetTasks()
        {
            ReadFromFileAsync();
            return _tasks;
        }
    }
}
