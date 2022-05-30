namespace GeoAppATM.Model
{
    /// <summary>
    /// Класс для хранения идентификатора и баланса в Json
    /// </summary>
    public class JsonATM
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Баланс
        /// </summary>
        public int Balance { get; set; }
    }
}
