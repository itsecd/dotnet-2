using Lab2.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Lab2.Repositories
{
    public class TaskRepository : ITaskRepository
    {

        private readonly string _storageFileName = "task.xml";
        private List<Task> _tasks;


        public TaskRepository(IConfiguration configuration = null)
        {
            if (configuration != null)
            {
                _storageFileName = configuration.GetValue<string>("TasksFile");
            }
            if (_tasks != null)
            {
                return;
            }

            if (!File.Exists(_storageFileName))
            {
                _tasks = new List<Task>();
                return;
            }
            var xmlSerializer = new XmlSerializer(typeof(List<Task>));
            using var fileReader = new FileStream(_storageFileName, FileMode.OpenOrCreate);
            _tasks = (List<Task>)xmlSerializer.Deserialize(fileReader);
        }

        public void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Task>));
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _tasks);
        }

        public int AddTask(Task tasks)
        {
            var maxId = _tasks.Max(t => t.TaskId);
            tasks.TaskId = maxId + 1;
            _tasks.Add(tasks);
            return tasks.TaskId;
        }

        public void RemoveAllTasks()
        {
            _tasks.RemoveRange(0, _tasks.Count);
        }

        public List<Task> GetTasks()
        {
            return _tasks;
        }

        public int RemoveTask(int id)
        {
            _tasks.RemoveAt(id);
            return id;
        }

        public int UpdateTask(int id, Task newTask)
        {
            var taskIndex = _tasks.FindIndex(p => p.TaskId == id);
            _tasks[taskIndex] = newTask;
            return id;
        }

    }
}