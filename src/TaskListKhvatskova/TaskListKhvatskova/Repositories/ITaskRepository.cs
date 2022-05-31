using TaskListKhvatskova.Models;
using System.Collections.Generic;

namespace TaskListKhvatskova.Repositories
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
        int AddTask(MyTask task);

        /// <summary>
        /// Мeтод получения всех задач 
        /// </summary>
        List<MyTask> GetTasks();

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
        int UpdateTask(int id, MyTask newTask);

        /// <summary>
        /// Мeтод изменения статуса задачи по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="newTask">Измененная задача</param>
        int UpdateTaskState(int id, bool state);

        /// <summary>
        /// Метод записи в файл
        /// </summary>
        void WriteToFile();

        /// <summary>
        /// Мeтод получения задачи по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Задача</returns>
        MyTask Get(int id);
    }
}