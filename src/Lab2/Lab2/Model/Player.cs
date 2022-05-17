namespace Lab2.Model
{
    public class Player
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        public string PlayerPosition { get; set; }

        public Player()
        {
        }

        public Player(string connectionId, string userName, string possition)
        {
            ConnectionId = connectionId;
            UserName = userName;
            PlayerPosition = possition;
        }
    }
}
