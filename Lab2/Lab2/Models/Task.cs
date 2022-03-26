
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
        public int Id { get; set; }
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
        /// кому назначена задача
        /// </summary>
        public Executor MemberOfTask { get; set; }
        /// <summary>
        /// набор тэгов, ассоциированных с задачей
        /// </summary>
        public List<Tags> TagsNames { get; set; }

    }
}
