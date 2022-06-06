using System.Drawing;

namespace TaskListKhvatskovaClient.Models
{
    /// <summary>
    /// Модель получения метки
    /// </summary>
    public class Tags
    {
        /// <summary>
        /// Id метки
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// Имя метки
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// цвет метки
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Tags()
        {
            Name = string.Empty;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Tags(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Tags(int id, string name, Color color)
        {
            TagId = id;
            Name = name;
            Color = color;
        }
    }
}

