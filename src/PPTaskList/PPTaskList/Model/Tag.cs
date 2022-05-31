using System;
using System.Collections.Generic;

namespace PPTask.Model
{
    /// <summary>
    /// Теги
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Название тега(приватное поле)
        /// </summary>
        private string _tagStatus;

        /// <summary>
        /// Цвет тега (приватное поле)
        /// </summary>
        private string _tagColour;

        /// <summary>
        /// Возможные статусы (приватное поле)
        /// </summary>
        private readonly List<string> _statuses = new List<string> { "Immediately", "Remake", "Finalize", "Done", "Not ready yet" };

        /// <summary>
        /// Возможные цвета (приватное поле)
        /// </summary>
        private readonly List<string> _colours = new List<string> { "Green", "Red", "Yellow" };

        /// <summary>
        /// Идентификатор тегов
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// Название тега
        /// </summary>
        public string TagStatus
        {
            get => _tagStatus;
            set
            {
                if (!_statuses.Contains(value))
                {
                    throw new ArgumentException("Tag status is not supported");
                }

                _tagStatus = value;
            }
        }

        /// <summary>
        /// Цвет тега
        /// </summary>
        public string TagColour
        {
            get => _tagColour;
            set
            {
                if (!_colours.Contains(value))
                {
                    throw new ArgumentException("Tag colour is not supported");
                }

                _tagColour = value;
            }
        }

        /// <summary>
        /// Конструкор по умолчанию
        /// </summary>
        public Tag() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Tag(int tagId, string tagStatus, string tagColour)
        {
            TagId = tagId;
            TagStatus = tagStatus;
            TagColour = tagColour;
        }
    }
}
