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
        /// Идентификатор исполнителя
        /// </summary>
        public int ExecutorId { get; set; }

        /// <summary>
        /// Список идентификаторов тегов
        /// </summary>
        public List<int> TagsId { get; set; }

    }
}
