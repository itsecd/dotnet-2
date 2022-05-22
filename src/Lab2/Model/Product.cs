namespace Lab2.Model
{
    /// <summary>
    /// Класс продукта
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Иденфикатор продукта
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название продукта               
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Цена продукта
        /// </summary>
        public float Price { get; set; }
    }
}
