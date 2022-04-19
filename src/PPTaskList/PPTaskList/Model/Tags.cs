using System.Collections.Generic;

namespace PPTask.Controllers.Model
{
    /// <summary>
    /// Теги
    /// </summary>
    public class Tags
    {
        /// <summary>
        /// Список возможных статусов
        /// </summary>
        private readonly List<string> _statuses = new List<string>
        {
            "Immediately", "Remake", "Finalize","Done", "Not ready yet"
        };

        /// <summary>
        /// Список возможных цветов тега
        /// </summary>
        private readonly List<string> _colours = new List<string>
        {
            "Green", "Red", "Yellow"
        };

        /// <summary>
        /// Список тегов (приватное поле)
        /// </summary>
        private List<string> _tags = new List<string> { };

        /// <summary>
        /// Список тегов
        /// </summary>
        public List<string> TagList 
        {
            get => _tags;
            set
            {
                for(var i = 0; i < value.Count; i++)
                {
                    if (_statuses.Exists(x => x == value[i]) || _colours.Exists(x => x == value[i]))
                    {
                        _tags.Add (value[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Конструкор по умолчанию
        /// </summary>
        public Tags() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Tags( List<string> tags)
        {
            TagList = tags;
        }
    }
}
