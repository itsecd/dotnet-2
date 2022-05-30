using Lab2.Models;
using System.Collections.Generic;

namespace Lab2.Repositories
{
    /// <summary>
    /// Интерфейс репозитория задач  
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Мeтод добавления задачи 
        /// </summary>
        /// <param name="task">Задача</param>
        ///<returns>Идентификатор задачи</returns>
        int AddTask(Task task);

        /// <summary>
        /// Мeтод получения всех задач 
        /// </summary>
        List<Task> GetTasks();

        /// <summary>
        /// Мeтод удаления всех задач
        /// </summary>
        void RemoveAllTasks();

        /// <summary>
        /// Мeтод удаления задачи по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Идентификатор задачи</returns>
        int RemoveTask(int id);

        /// <summary>
        /// Мeтод изменения задачи по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="newTask">Измененная задача</param>
        int UpdateTask(int id, Task newTask);

        /// <summary>
        /// Метод записи в файл
        /// </summary>
        void WriteToFile();

        /// <summary>
        /// Мeтод получения задачи по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Задача</returns>
        Task Get(int id);

        List<Tags> GetTags(int id);
        int AddTag(int id, Tags tag);
        Tags GetTag(int id, int num);
        int RemoveTag(int id, int num);
        int UpdateTag(int id, int num, Tags newTag);
        int RemoveAllTags(int id);
    }
}
