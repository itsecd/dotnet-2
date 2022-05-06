using System.Collections.Generic;

namespace Lab2.Dto
{
    /// <summary>
    /// Исполнитель задачи
    /// </summary>
    public class TaskDto
    {
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

    }
}