using Microsoft.Extensions.Configuration;
using PPTask.Controllers.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PPTask.Repositories
{
    /// <summary>
    /// Класс сериализации и десериализации задач в формате json 
    /// </summary>
    public class JsonTaskRepository : ITaskRepository
    {
        /// <summary>
        /// Название файла хранения
        /// </summary>
        private readonly string _storageFileName;

        /// <summary>
        /// Список задач
        /// </summary>
        private List<Task> _tasks;

        /// <summary>
        /// Получение файла хранения
        /// </summary>
        public JsonTaskRepository(IConfiguration configuration = null)
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
            
            var repositoryJson = File.ReadAllText(_storageFileName);
            _tasks = JsonSerializer.Deserialize<List<Task>>(repositoryJson);
        }

        /// <summary>
        /// Асинхронный метод записи в файл
        /// </summary>
        /// <returns> Task </returns>
        public async System.Threading.Tasks.Task WriteToFileAsync()
        {
            await using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            await JsonSerializer.SerializeAsync<List<Task>>(fileWriter, _tasks);
        }

        /// <summary>
        /// Метод добавления задачи
        /// </summary>
        /// <param name="task">Задача</param>
        public void AddTask(Task task)
        {
            if (_tasks.Count == 0)
            {
                task.TaskId = 1;
            }
            else
            {
                var maxId = _tasks.Max(t => t.TaskId);
                task.TaskId = maxId + 1;
            }
            _tasks.Add(task);
        }

        /// <summary>
        /// Метод удаления задачи  
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        public void RemoveTask(int id)
        {
            if (_tasks != null)
            {
                _tasks.RemoveAll(task => task.TaskId == id);
            }
        }

        /// <summary>
        /// Метод удаления всех задач  
        /// </summary>
        public void RemoveAllTasks()
        {
            if (_tasks != null)
            {
                _tasks.RemoveRange(0, _tasks.Count());
            }
        }

        /// <summary>
        /// Метод получения всех задач 
        /// </summary>
        /// <returns>Список задач</returns>
        public List<Task> GetTasks()
        {
            return _tasks;
        }
    }
}
