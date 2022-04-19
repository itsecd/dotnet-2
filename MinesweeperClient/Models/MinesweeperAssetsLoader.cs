using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace MinesweeperClient.Models
{
    public class MinesweeperAssets
    {
        Dictionary<string, Bitmap> _assets = new();
        string[] _filenames = {"Flag.png", "Bomb.png", "BombMarked.png", "AppIcon.png"};
        public MinesweeperAssets()
        {
        }
    }
}