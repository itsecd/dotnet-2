using Microsoft.Extensions.Configuration;
using PPTask.Controllers.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;



namespace PPTask.Repositories
{
    /// <summary>
    /// Класс сериализации и десериализации исполнителей в формате json 
    /// </summary>
    public class JsonExecutorRepository: TimedHostedService, IExecutorRepository
    {
        /// <summary>
        /// Название файла хранения
        /// </summary>
        private readonly string _storageFileName;

        /// <summary>
        /// Получение файла хранения
        /// </summary>
        public JsonExecutorRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("ExecutorsFile");
        }

        /// <summary>
        /// Список исполнителей
        /// </summary>
        private List<Executor> _executors;


        public override void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            WriteToFileAsync();
        }


        /// <summary>
        /// Асинхронный метод чтения из файла
        /// </summary>
        /// <returns> Task </returns>
        private async System.Threading.Tasks.Task ReadFromFileAsync()
        {
            if (_executors != null) return;

            if (!File.Exists(_storageFileName))
            {
                _executors = new List<Executor>();
                return;
            }

            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _executors = await JsonSerializer.DeserializeAsync<List<Executor>>(fileReader);
        }

        /// <summary>
        /// Асинхронный метод записи в файл
        /// </summary>
        /// <returns> Task </returns>
        private async System.Threading.Tasks.Task WriteToFileAsync()
        {
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            await JsonSerializer.SerializeAsync<List<Executor>>(fileWriter, _executors);
        }

        /// <summary>
        /// Мнтод добавления исполнителя 
        /// </summary>
        /// <param name="executor">Исполнитель</param>
        /// <returns> Task </returns>
        public async System.Threading.Tasks.Task AddExecutor(Executor executor)
        {
            await ReadFromFileAsync();
            _executors.Add(executor);
            //await WriteToFileAsync();
        }

        /// <summary>
        /// Мнтод удаления исполнителя  
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        ///  <returns> Task </returns>
        public async System.Threading.Tasks.Task RemoveExecutor(int id)
        {
            if (_executors != null)
            {
                await ReadFromFileAsync();
                _executors.RemoveAll(executor => executor.ExecutorId == id);
                //await WriteToFileAsync();
            }
        }

        /// <summary>
        /// Мнтод получения всех исполнителей 
        /// </summary>
        /// <returns>Список исполнителей</returns>
        public async System.Threading.Tasks.Task<List<Executor>> GetExecutors()
        {
            await ReadFromFileAsync();
            return _executors;
        }
    }
}
