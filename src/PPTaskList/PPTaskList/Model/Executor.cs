namespace PPTask.Controllers.Model
{
    /// <summary>
    /// Исполнитель задачи
    /// </summary>
    public class Executor
    {
        /// <summary>
        /// Идентификатор исполнителя
        /// </summary>
        public int ExecutorId { get; set; }

        /// <summary>
        /// Имя исполнителя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Конструкор по умолчанию
        /// </summary>
        public Executor() {}

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Executor(int id, string name)
        {
            ExecutorId = id;
            Name = name;
        }
    }
}
