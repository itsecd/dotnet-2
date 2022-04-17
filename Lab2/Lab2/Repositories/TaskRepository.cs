using Lab2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Lab2.Repositories
{
	public class TaskRepository: ITaskRepository
	{
        private readonly string _storageFileName;

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

        public void SaveFile()
        {
            WriteToFile();
        }
        public int AddTask(TaskList tasks)
        {
            ReadFromFile();
            _tasks.Add(tasks);
            WriteToFile();
            return tasks.TaskId;
        }

        public void RemoveAllTasks()
        {
            ReadFromFile();
            _tasks.RemoveRange(0, _tasks.Count);
            WriteToFile();

        }

        public List<TaskList> GetTasks()
        {
            ReadFromFile();
            return _tasks;
        }

        public int RemoveTask(int id)
        {
            ReadFromFile();
            _tasks.RemoveAt(id);
            WriteToFile();
            return id; 
        }
    }
}