namespace SnakeServer.Database
{
    /// <summary>Класс для хранения информации об игроке.</summary>
    public class Player
    {
        public int TotalScore { get; set; }
        public int WinCount { get; set; }
        public int LoseCount { get; set; }
    }
}