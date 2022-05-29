namespace Lab2.Models
{
    /// <summary>
    /// Модель исполнителя задачи задачи
    /// </summary>
    public class Executor
    {
        /// <summary>
        /// id исполнителя
        /// </summary>
        public int ExecutorId { get; set; }

        /// <summary>
        /// имя исполнителя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// фамилия исполнителя
        /// </summary>
        public string Surname { get; set; }

    }
}
