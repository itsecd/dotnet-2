using System.Collections.Generic;

namespace PPTask.Controllers.Model
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
        /// Цвет тега (привfтное поле)
        /// </summary>
        private string _tagColour;

        /// <summary>
        /// Идентификатор тегов
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// Название тега
        /// </summary>
        public string TagStatus 
        {
            get { return _tagStatus; }
            set
            {
                var statuses = new List<string> { "Immediately", "Remake", "Finalize", "Done", "Not ready yet" };
                foreach (var stat in statuses)
                {
                    if (value == stat)
                    {
                        _tagStatus = value;
                    }
                }
            } 
        }

        /// <summary>
        /// Цвет тега
        /// </summary>
        public string TagColour 
        {
            get { return _tagColour; }
            set
            {
                var colour = new List<string> { "Green", "Red", "Yellow" };
                foreach (var c in colour)
                {
                    if (value == c)
                    {
                        _tagColour = value;
                    }
                }
            }
        }

        /// <summary>
        /// Конструкор по умолчанию
        /// </summary>
        public Tag() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Tag(int tagId, string tagStatus, string tagColour )
        {
            TagId = tagId;
            TagStatus = tagStatus;
            TagColour = tagColour;
        }
    }
}
