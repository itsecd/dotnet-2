using PPTask.Controllers.Model;
using System.Collections.Generic;

namespace PPTask.Repositories
{
    /// <summary>
    /// Интерфейс репозиторий задач  
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Метод добавления задачи
        /// </summary>
        /// <param name="task">Задача</param>
        /// <returns> Task </returns>
        void AddTask(Task task);

        /// <summary>
        /// Метод получения всех задач 
        /// </summary>
        /// <returns>Список задач</returns>
        List<Task> GetTasks();

        /// <summary>
        /// Метод удаления задачи  
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        ///  <returns> Task </returns>
        void RemoveTask(int id);

        /// <summary>
        /// Метод записи данных в файл 
        /// </summary>
        System.Threading.Tasks.Task WriteToFileAsync();
    }
}
