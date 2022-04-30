using Lab2.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Lab2.Repositories
{
	public class TaskRepository: ITaskRepository
	{

        private readonly string _storageFileName;

        public TaskRepository(){ }

         public TaskRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("TasksFile");
        }

        private List<TaskList> _tasks;

        private async Task ReadFromFile()
        {
            if (_tasks != null) return;

            if (!File.Exists(_storageFileName))
            {
                _tasks = new List<TaskList>();
                return;
            }
            await DeserializeFile();     
        }

        private async Task DeserializeFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<TaskList>));
            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _tasks = (List<TaskList>)xmlSerializer.Deserialize(fileReader);
        }

        private async Task SerializeFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<TaskList>));
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _tasks);
        }

        private async Task WriteToFile()
        {
            await SerializeFile();
        }

        public async Task SaveFile()
        {
            await WriteToFile();
        }

        public async Task<int> AddTask(TaskList tasks)
        {
            await ReadFromFile();
            _tasks.Add(tasks);
            await WriteToFile();
            return tasks.TaskId;
        }

        public async Task RemoveAllTasks()
        {
            await ReadFromFile();
            _tasks.RemoveRange(0, _tasks.Count);
            await WriteToFile();

        }

        public async Task<List<TaskList>> GetTasks()
        {
            await ReadFromFile();
            return _tasks;
        }

        public async Task<int> RemoveTask(int id)
        {
            await ReadFromFile();
            _tasks.RemoveAt(id);
            await WriteToFile();
            return id; 
        }
    }
}