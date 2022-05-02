using PPTask.Controllers.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task<List<Executor>> GetExecutors();

        /// <summary>
        /// Метод добавления исполнителя 
        /// </summary>
        /// <param name="executor">Исполнитель</param>
        /// <returns> Task </returns>
        System.Threading.Tasks.Task AddExecutor(Executor executor);

        /// <summary>
        /// Метод удаления исполнителя  
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        ///  <returns> Task </returns>
        System.Threading.Tasks.Task RemoveExecutor(int id);

    }
}
