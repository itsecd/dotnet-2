using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TaskListKhvatskova.Models;

namespace TaskListKhvatskova.Repositories
{
    /// <summary>
    /// Класс для тэга с добавлением, удалением, изменением, сериализацией и десериализацией исполнителей в формате xml 
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        /// <summary>
        /// Список тэгов
        /// </summary>
        private readonly List<MyTask> _tasks;

        /// <summary>
        /// Название файла хранения
        /// </summary>
        private readonly string _storageFileName = "task.json";

        public TaskRepository()
        {
            _tasks = new List<MyTask>();
        }

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
                _tasks = new List<MyTask>();
                return;
            }

            string jsonString = File.ReadAllText(_storageFileName);
            _tasks = JsonSerializer.Deserialize<List<MyTask>>(jsonString);
        }

        /// <summary>
        /// Метод записи в файл
        /// </summary>
        public void WriteToFile()
        {
            string jsonString = JsonSerializer.Serialize(_tasks);
            File.WriteAllText(_storageFileName, jsonString);
        }

        /// <summary>
        /// Мeтод получения задачи по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Задача</returns>
        public MyTask Get(int id) => _tasks.FirstOrDefault(p => p.TaskId == id);

        /// <summary>
        /// Мeтод добавления задачи 
        /// </summary>
        /// <param name="task">Задача</param>
        ///<returns>Идентификатор задачи</returns>
        public int AddTask(MyTask task)
        {
            if (task.TaskId == default)
            {
                if (_tasks.Count == 0)
                {
                    task.TaskId = 1;
                    _tasks.Add(task);
                }
                else
                {
                    task.TaskId = _tasks.Max(t => t.TaskId) + 1;
                    _tasks.Add(task);
                }
            }
            else
            {
                if (_tasks.FindIndex(t => t.TaskId == task.TaskId) == -1)
                {
                    _tasks.Add(task);
                }
                else
                {
                    throw new Exception("This ID already exists");
                }
            }
            WriteToFile();
            return task.TaskId;

        }

        /// <summary>
        /// Мeтод удаления всех задач
        /// </summary>
        public void RemoveAllTasks()
        {
            _tasks.RemoveRange(0, _tasks.Count);
            WriteToFile();
        }

        /// <summary>
        /// Мeтод получения всех задач 
        /// </summary>
        public List<MyTask> GetTasks()
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
            var deletedTask = Get(id);
            _tasks.Remove(deletedTask);
            WriteToFile();
            return id;
        }

        /// <summary>
        /// Мeтод изменения задачи по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="newTask">Измененная задача</param>
        public int UpdateTask(int id, MyTask newTask)
        {
            var taskIndex = _tasks.FindIndex(p => p.TaskId == id);
            _tasks[taskIndex] = newTask;
            WriteToFile();
            return id;
        }

        /// <summary>
        /// Мeтод изменения статуса задачи по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="state">Измененная задача</param>
        public int UpdateTaskState(int id, bool state)
        {
            _tasks.Find(p => p.TaskId == id).TaskState = state;
            WriteToFile();
            return id;
        }

    }
}