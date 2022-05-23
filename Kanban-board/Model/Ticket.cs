namespace Kanban_board.Model
{
    public class Ticket
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Название задачи
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Подробное описание задачи
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Id статуса, по которому можно получить статус задачи
        /// </summary>
        public string StatusId { get; set; }
    }
}
