namespace Kanban_board.Model
{

    public class Status
    {
        /// <summary>
        /// Идентификатор статуса
        /// </summary>
        public string Id { get; init; }
        /// <summary>
        /// Название статуса
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание статуса, чтобы подписать, какие задачи характерны для этого статуса
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Приоритет статуса (цвет, которым будет подсвечена соответствующая колонка)
        /// </summary>
        public string Priority { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Status status &&
                status.Id == this.Id &&
                status.Name == this.Name &&
                status.Description == this.Description &&
                status.Priority == this.Priority;
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
