using System;

namespace TaskListKhvatskovaClient.Models
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
        public string TagsId { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>

        public MyTask()
        {
            TagsId = string.Empty;
            ExecutorId = 0;
            Name = string.Empty;
            Description = string.Empty;
            TaskState = false;
        }

        public MyTask(int taskId, string name, string description, bool state, int executorId, string tagsId)
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
