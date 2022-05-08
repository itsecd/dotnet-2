using System.Collections.Generic;

namespace PPTask.Dto
{
    /// <summary>
    /// Теги
    /// </summary>
    public class TagDto
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
    }
}
