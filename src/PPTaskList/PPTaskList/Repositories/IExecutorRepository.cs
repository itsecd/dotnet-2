using PPTask.Model;
using System.Collections.Generic;

namespace PPTask.Repositories
{
    /// <summary>
    /// Интерфейс репозиторий исполнителей  
    /// </summary>
    public interface IExecutorRepository
    {
        /// <summary>
        /// Метод получения всех исполнителей 
        /// </summary>
        /// <returns>Список исполнителей</returns>
        List<Executor> GetExecutors();

        /// <summary>
        /// Метод добавления исполнителя 
        /// </summary>
        /// <param name="executor">Исполнитель</param>
        void AddExecutor(Executor executor);

        /// <summary>
        /// Метод удаления исполнителя  
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        void RemoveExecutor(int id);

        /// <summary>
        /// Метод удаления всех исполнителей  
        /// </summary>
        void RemoveAllExecutors();

        /// <summary>
        /// Метод записи данных в файл 
        /// </summary>
        System.Threading.Tasks.Task WriteToFileAsync();

    }
}
