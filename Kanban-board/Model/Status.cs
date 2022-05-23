namespace Kanban_board.Model
{

    public class Status
    {
        /// <summary>
        /// Идентификатор статуса
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Название статуса
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание статуса, чтобы подписать, какие задачи характерны для этого статуса
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Приоритет статуса (цвет, которым будет подсвечера соответствующая колонка)
        /// </summary>
        public string Priority { get; set; }
    }
}
