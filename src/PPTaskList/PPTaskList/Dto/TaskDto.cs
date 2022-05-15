using System.Collections.Generic;
using PPTask.Model;

namespace PPTask.Dto
{
    /// <summary>
    /// Задачи 
    /// </summary>
    public class TaskDto
    {
        /// <summary>
        /// Заголовок задачи
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// Описание задачи
        /// </summary>
        public string TextDescription { get; set; }

        /// <summary>
        /// Исполнитель назначенный на задачу
        /// </summary>
        public Executor Executor { get; set; }

        /// <summary>
        /// Список идентификаторов тегов
        /// </summary>
        public List<int> TagsId { get; set; }

    }
}
