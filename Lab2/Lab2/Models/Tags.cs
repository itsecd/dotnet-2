namespace Lab2.Models
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
        public int Color { get; set; }
        public Tags()
        {
            Name = string.Empty;
        }
        public Tags(string name, int color)
        {
            Name = name;
            Color = color;
        }
    }
}
