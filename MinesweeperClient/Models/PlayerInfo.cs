namespace MinesweeperClient.Models
{
    public class PlayerInfo
    {
        public string Name { get; set; }
        public int PlayCount { get; set; }
        public int WinCount { get; set; }
        public int WinStreak { get; set; }
        public string Stats { get => $"{Name} {PlayCount}/{WinCount}/{WinStreak}"; }
    }
}