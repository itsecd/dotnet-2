
using System.Collections.Generic;


namespace Lab2.Models
{
    /// <summary>
    /// Модель запроса задачи
    /// </summary>
    public class Task
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
        /// Конструкор по умолчанию
        /// </summary>
        public Task()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Task(string name, string description, int executorId, List<int> tagsId)
        {
            Name = name;
            Description = description;
            ExecutorId = executorId;
            TagsId = tagsId;
        }
    }
}
