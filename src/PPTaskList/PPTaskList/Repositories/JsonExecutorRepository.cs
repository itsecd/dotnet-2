using Microsoft.Extensions.Configuration;
using PPTask.Controllers.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PPTask.Repositories
{
    /// <summary>
    /// Класс сериализации и десериализации исполнителей в формате json 
    /// </summary>
    public class JsonExecutorRepository: IExecutorRepository
    {
        /// <summary>
        /// Название файла хранения
        /// </summary>
        private readonly string _storageFileName;

        /// <summary>
        /// Список исполнителей
        /// </summary>
        private List<Executor> _executors;

        /// <summary>
        /// Получение файла хранения
        /// </summary>
        public JsonExecutorRepository(IConfiguration configuration = null)
        {
            if (configuration != null)
            {
                _storageFileName = configuration.GetValue<string>("ExecutorsFile");
            }

            if (!File.Exists(_storageFileName))
            {
                _executors = new List<Executor>();
                return;
            }

            var repositoryJson = File.ReadAllText(_storageFileName);
            _executors = JsonSerializer.Deserialize<List<Executor>>(repositoryJson);
        }

        /// <summary>
        /// Асинхронный метод записи в файл
        /// </summary>
        /// <returns> Task </returns>
        public async System.Threading.Tasks.Task WriteToFileAsync()
        {
            await using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            await JsonSerializer.SerializeAsync<List<Executor>>(fileWriter, _executors);
        }

        /// <summary>
        /// Мeтод добавления исполнителя 
        /// </summary>
        /// <param name="executor">Исполнитель</param>
        public void AddExecutor(Executor executor)
        {
            if (_executors.Count == 0)
            {
                executor.ExecutorId = 1;
            }
            else
            {
                var maxId = _executors.Max(ex => ex.ExecutorId);
                executor.ExecutorId = maxId + 1;
            }
            _executors.Add(executor);
        }

        /// <summary>
        /// Мeтод удаления исполнителя  
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        public void RemoveExecutor(int id)
        {
            if (_executors != null)
            {
                _executors.RemoveAll(executor => executor.ExecutorId == id);
            }
        }

        /// <summary>
        /// Мeтод удаления всех исполнителей  
        /// </summary>
        public void RemoveAllExecutors()
        {
            if (_executors != null)
            {
                _executors.RemoveRange(0, _executors.Count());
            }
        }

        /// <summary>
        /// Мeтод получения всех исполнителей 
        /// </summary>
        /// <returns>Список исполнителей</returns>
        public List<Executor> GetExecutors()
        {
            return _executors;
        }
    }
}
