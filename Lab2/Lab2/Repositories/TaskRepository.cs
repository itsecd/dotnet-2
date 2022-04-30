using Lab2.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
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

        private List<Task> _tasks;

        private void ReadFromFile()
        {
            if (_tasks != null) return;

            if (!File.Exists(_storageFileName))
            {
                _tasks = new List<Task>();
                return;
            }
            var xmlSerializer = new XmlSerializer(typeof(List<Task>));
            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _tasks = (List<Task>)xmlSerializer.Deserialize(fileReader);
        }

        private void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Task>));
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _tasks);
        }

        public int AddTask(Task tasks)
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

        public List<Task> GetTasks()
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

        public int UpdateTask(int id, Task newTask)
        {
            ReadFromFile();
            var taskIndex = _tasks.FindIndex(p => p.TaskId == id);
            _tasks[taskIndex] = newTask;
            WriteToFile();
            return id;
        }

    }
}