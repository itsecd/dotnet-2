using Lab2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System.Threading.Tasks;


namespace Lab2.Repositories
{
	public class TaskRepository: ITaskRepository
	{
        private const string StorageFileName = "tasks.xml";

        private List<TaskList> _tasks;

        private void ReadFromFile()
        {
            if (_tasks != null) return;

            if (!File.Exists(StorageFileName))
            {
                _tasks = new List<TaskList>();
                return;
            }

            var xmlSerializer = new XmlSerializer(typeof(List<TaskList>));
            using var fileReader = new FileStream(StorageFileName, FileMode.Open);
            _tasks = (List<TaskList>)xmlSerializer.Deserialize(fileReader);
        }
        private void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<TaskList>));
            using var fileWriter = new FileStream(StorageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _tasks);
        }

        public void AddTask(TaskList tasks)
        {
            ReadFromFile();
            _tasks.Add(tasks);
            WriteToFile();
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


    }
}