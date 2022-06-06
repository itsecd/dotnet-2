using System;
using System.Collections.Generic;

namespace TaskListKhvatskova.Models
{
    /// <summary>
    /// Модель запроса задачи
    /// </summary>
    [Serializable]
    public class MyTask
    {
        /// <summary>
        /// id задачи
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// имя задачи
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// описание задачи
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// статус завершенности задачи
        /// </summary>
        public bool TaskState { get; set; }

        /// <summary>
        /// Идентификатор исполнителя
        /// </summary>
        public int ExecutorId { get; set; }

        /// <summary>
        /// Список идентификаторов тегов
        /// </summary>
        public List<int> TagsId { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public MyTask()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public MyTask(string name, string description, int executorId, List<int> tagsId)
        {
            Name = name;
            Description = description;
            ExecutorId = executorId;
            TagsId = tagsId;
        }

        public MyTask(int taskId, string name, string description, bool state, int executorId, List<int> tagsId)
        {
            TaskId = taskId;
            Name = name;
            Description = description;
            TaskState = state;
            ExecutorId = executorId;
            TagsId = tagsId;
        }
    }
}
