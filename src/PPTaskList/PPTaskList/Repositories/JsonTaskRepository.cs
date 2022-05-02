using Microsoft.Extensions.Configuration;
using PPTask.Controllers.Model;
using System.Collections.Generic;
using System.IO;
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
        /// Получение файла хранения
        /// </summary>
        public JsonTaskRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("TasksFile");
        }

        /// <summary>
        /// Список задач
        /// </summary>
        private List<Task> _tasks;

        /// <summary>
        /// Асинхронный метод чтения из файла
        /// </summary>
        /// <returns> Task </returns>
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

        /// <summary>
        /// Асинхронный метод записи в файл
        /// </summary>
        /// <returns> Task </returns>
        private async System.Threading.Tasks.Task WriteToFileAsync()
        {
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            await JsonSerializer.SerializeAsync<List<Task>>(fileWriter, _tasks);
        }

        /// <summary>
        /// Метод добавления задачи
        /// </summary>
        /// <param name="task">Задача</param>
        /// <returns> Task </returns>
        public async System.Threading.Tasks.Task AddTask(Task task)
        {
            await ReadFromFileAsync();
            _tasks.Add(task);
            await WriteToFileAsync();
        }

        /// <summary>
        /// Метод удаления задачи  
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        ///  <returns> Task </returns>
        public async System.Threading.Tasks.Task RemoveTask(int id)
        {
            if (_tasks != null)
            {
                await ReadFromFileAsync();
                _tasks.RemoveAll(task => task.TaskId == id);
                await WriteToFileAsync();
            }
        }

        /// <summary>
        /// Метод получения всех задач 
        /// </summary>
        /// <returns>Список задач</returns>
        public async System.Threading.Tasks.Task<List<Task>> GetTasks()
        {
            await ReadFromFileAsync();
            return _tasks;
        }
    }
}
