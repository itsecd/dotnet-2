using System.Collections.Generic;

namespace SudokuServer.Models
{
    public sealed class Player
    {
        public string Login { get; set; } = string.Empty;
        public HashSet<int> Solved { get; set; } = new HashSet<int>();

    }
}
