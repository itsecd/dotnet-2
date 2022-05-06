using Lab2.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Lab2.Repositories
{
    /// <summary>
    /// Класс для тэга с добавлением, удалением, изменением, сериализацией и десериализацией исполнителей в формате xml 
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        /// <summary>
        /// Название файла хранения
        /// </summary>
        private readonly string _storageFileName = "task.xml";

        /// <summary>
        /// Список тэгов
        /// </summary>
        private readonly List<Task> _tasks;

        /// <summary>
        /// Файл хранения
        /// </summary>
        public TaskRepository(IConfiguration configuration = null)
        {
            if (configuration != null)
            {
                _storageFileName = configuration.GetValue<string>("TasksFile");
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

        /// <summary>
        /// Метод записи в файл
        /// </summary>
        public void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Task>));
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _tasks);
        }

        /// <summary>
        /// Мeтод получения задачи по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Задача</returns>
        public Task Get(int id) => _tasks.FirstOrDefault(p => p.TaskId == id);

        /// <summary>
        /// Мeтод добавления задачи 
        /// </summary>
        /// <param name="task">Задача</param>
        ///<returns>Идентификатор задачи</returns>
        public int AddTask(Task tasks)
        {
            var id = tasks.TaskId;
            if (tasks.TaskId == default)
            {
                if (_tasks.Count == 0)
                {
                    tasks.TaskId = 1;
                    _tasks.Add(tasks);
                }
                else
                {
                    tasks.TaskId = _tasks.Max(t => t.TaskId) + 1;
                    _tasks.Add(tasks);
                }
            }
            else
            {
                var maxId = _tasks.Max(t => t.TaskId);
                tasks.TaskId = maxId + 1;
                _tasks.Add(tasks);
            }
            return id;
        }

        /// <summary>
        /// Мeтод удаления всех задач
        /// </summary>
        public void RemoveAllTasks()
        {
            _tasks.RemoveRange(0, _tasks.Count);
        }

        /// <summary>
        /// Мeтод получения всех задач 
        /// </summary>
        public List<Task> GetTasks()
        {
            return _tasks;
        }

        /// <summary>
        /// Мeтод удаления задачи по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Идентификатор задачи</returns>
        public int RemoveTask(int id)
        {
            var DeletedTask = Get(id);
            _tasks.Remove(DeletedTask);
            return id;
        }

        /// <summary>
        /// Мeтод изменения задачи по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="newTask">Измененная задача</param>
        public int UpdateTask(int id, Task newTask)
        {
            var taskIndex = _tasks.FindIndex(p => p.TaskId == id);
            _tasks[taskIndex] = newTask;
            return id;
        }

    }
}