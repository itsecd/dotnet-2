namespace Lab2.Model
{
    public class Player
    {
        public string? ConnectionId { get; set; }
        public string? UserName { get; set; }
        public string? PlayerPosition { get; set; }

        public Player()
        {

        }

        public Player(string connectionId, string userName, string possition)
        {
            ConnectionId = connectionId;
            UserName = userName;
            PlayerPosition = possition;
        }

        public override bool Equals(object? obj)
        {
            if(obj is not Player player) return false;
            return UserName == player.UserName && PlayerPosition == player.PlayerPosition && ConnectionId == player.ConnectionId;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
