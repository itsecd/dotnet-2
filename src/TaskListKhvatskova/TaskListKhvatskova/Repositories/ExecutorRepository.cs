using TaskListKhvatskova.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Text.Json;


namespace TaskListKhvatskova.Repositories
{
    /// <summary>
    /// Класс для исполнителя задач с добавлением, удалением, изменением, сериализацией и десериализацией исполнителей в формате xml 
    /// </summary>
    public class ExecutorRepository : IExecutorRepository
    {
        /// <summary>
        /// Название файла хранения
        /// </summary>
        private readonly string _storageFileName = "executors.json";

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

            string jsonString = File.ReadAllText(_storageFileName);
            _executors = JsonSerializer.Deserialize<List<Executor>>(jsonString);
        }

        /// <summary>
        /// Мeтод получения исполнителя по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        /// <returns>Исполнитель задачи</returns>
        public Executor Get(int id)
        {
            Executor e = new Executor();
            foreach (Executor tmp in _executors)
            {
                if (tmp.ExecutorId == id)
                {
                    e = tmp;
                }
            }
            return e;
        }
        public void WriteToFile()
        {
            string jsonString = JsonSerializer.Serialize(_executors);
            File.WriteAllText(_storageFileName, jsonString);
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
            else
            {
                if (_executors.FindIndex(ex => ex.ExecutorId == executor.ExecutorId) == -1)
                {
                    _executors.Add(executor);
                }
                else
                {
                    throw new Exception("This ID already exists");
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
            var deletedExecutor = Get(id);
            _executors.Remove(deletedExecutor);
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

