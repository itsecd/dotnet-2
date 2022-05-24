namespace Kanban_board.Model
{
    public class Ticket
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public string Id { get; init; }
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

        public override bool Equals(object obj)
        {
            return obj is Ticket ticket &&
                ticket.Id == Id &&
                ticket.Title == Title &&
                ticket.Description == Description;
        }

        public override int GetHashCode()
        {
            try
            {
                return int.Parse(Id);
            }
            catch
            {
                return 0;
            }
        }
    }
}
