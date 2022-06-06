using System.Collections.Generic;
using TaskListKhvatskova.Models;

namespace TaskListKhvatskova.Repositories
{
    /// <summary>
    /// Интерфейс репозитория исполнителей  
    /// </summary>
    public interface IExecutorRepository
    {
        /// <summary>
        /// Мeтод получения всех исполнителей 
        /// </summary>
        /// <returns>Список исполнителей</returns>
        List<Executor> GetExecutors();

        /// <summary>
        /// Мeтод добавления исполнителя 
        /// </summary>
        /// <param name="executor">Исполнитель</param>
        ///<returns>Идентификатор исполнителя задачи</returns>
        int AddExecutor(Executor executor);

        /// <summary>
        /// Мeтод удаления всех исполнителей
        /// </summary>
        void RemoveAllExecutors();

        /// <summary>
        /// Мeтод удаления исполнителя по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        /// <returns>Идентификатор исполнителя</returns>
        int RemoveExecutor(int id);

        /// <summary>
        /// Мeтод изменения исполнителя по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        /// <param name="newExecutor">Измененный исполнитель</param>
        int UpdateExecutor(int id, Executor newExecutor);

        /// <summary>
        /// Метод записи в файл
        /// </summary>
        void WriteToFile();

        /// <summary>
        /// Мeтод получения исполнителя по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        /// <returns>Исполнитель задачи</returns>
        Executor Get(int id);

    }
}
