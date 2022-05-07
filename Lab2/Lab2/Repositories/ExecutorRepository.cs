using Lab2.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;


namespace Lab2.Repositories
{
    /// <summary>
    /// Класс для исполнителя задач с добавлением, удалением, изменением, сериализацией и десериализацией исполнителей в формате xml 
    /// </summary>
    public class ExecutorRepository : IExecutorRepository
    {
        /// <summary>
        /// Название файла хранения
        /// </summary>
        private readonly string _storageFileName = "executors.xml";

        /// <summary>
        /// Список исполнителей
        /// </summary>
        private readonly List<Executor> _executors;


        /// <summary>
        /// Файл хранения
        /// </summary>
        public ExecutorRepository(IConfiguration configuration = null)
        {
            if (configuration is not null)
            {
                _storageFileName = configuration.GetValue<string>("ExecutorsFile");
            }

            if (!File.Exists(_storageFileName))
            {
                _executors = new List<Executor>();
                return;
            }
            var xmlSerializer = new XmlSerializer(typeof(List<Executor>));
            using var fileReader = new FileStream(_storageFileName, FileMode.Open);
            _executors = (List<Executor>)xmlSerializer.Deserialize(fileReader);
        }

        /// <summary>
        /// Мeтод получения исполнителя по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        /// <returns>Исполнитель задачи</returns>
        public Executor Get(int id) => _executors.FirstOrDefault(p => p.ExecutorId == id);

        /// <summary>
        /// Метод записи в файл
        /// </summary>
        public void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Executor>));
            using var fileWriter = new FileStream(_storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileWriter, _executors);
        }


        /// <summary>
        /// Мeтод добавления исполнителя 
        /// </summary>
        /// <param name="executor">Исполнитель</param>
        ///<returns>Идентификатор исполнителя задачи</returns>
        public int AddExecutor(Executor executor)
        {
            if (executor.ExecutorId == default)
            {
                if (_executors.Count == 0)
                {
                    executor.ExecutorId = 1;
                    _executors.Add(executor);
                }
                else
                {
                    executor.ExecutorId = _executors.Max(ex => ex.ExecutorId) + 1;
                    _executors.Add(executor);
                }
            }
            return executor.ExecutorId;
        }

        /// <summary>
        /// Мeтод удаления всех исполнителей
        /// </summary>
        public void RemoveAllExecutors()
        {
            _executors.RemoveRange(0, _executors.Count);
        }

        /// <summary>
        /// Мeтод получения всех исполнителей 
        /// </summary>
        /// <returns>Список исполнителей</returns>
        public List<Executor> GetExecutors()
        {
            return _executors;
        }

        /// <summary>
        /// Мeтод удаления исполнителя по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        /// <returns>Идентификатор исполнителя</returns>
        public int RemoveExecutor(int id)
        {
            var DeletedExecutor = Get(id);
            _executors.Remove(DeletedExecutor);
            return id;
        }

        /// <summary>
        /// Мeтод изменения исполнителя по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        /// <param name="newExecutor">Измененный исполнитель</param>
        public int UpdateExecutor(int id, Executor newExecutor)
        {
            var executorIndex = _executors.FindIndex(p => p.ExecutorId == id);
            _executors[executorIndex] = newExecutor;
            return id;
        }

    }
}
