namespace TaskListKhvatskovaClient.Models
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


        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Executor()
        {
            Name = string.Empty;
            Surname = string.Empty;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Executor(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Executor(int id, string name, string surname)
        {
            ExecutorId = id;
            Name = name;
            Surname = surname;
        }
    }
}
