using System;

// 0 - 9 количество мин вокруг клетки
// -1 мина

// 0 закрыто
// 1 открыто
// 2 флажок

namespace MinesweeperClient.Models
{
    /// <summary>Поле для игры "Сапёр".</summary>
    public class MinesweeperField
    {
        /// <summary>Ширина поля.</summary>
        public int Width;
        /// <summary>Высота поля.</summary>
        public int Height;
        private int[,] _field;
        private RevealStates[,] _rev;
        public int this[int x, int y] => _field[x, y];

        /// <summary>Создание поля определённого размера.</summary>
        public MinesweeperField(int height, int width)
        {
            Height = height;
            Width = width;
            _field = new int[Width, Height];
            _rev = new RevealStates[Width, Height];
        }
        /// <summary>Раскрытие расположения всех мин.</summary>
        public void RevealMines()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    if (_field[x, y] == -1 && _rev[x, y] == RevealStates.Closed)
                        _rev[x, y] = RevealStates.Opened;
                    else if (_field[x, y] == -1 && _rev[x, y] == RevealStates.Flagged)
                        _rev[x, y] = RevealStates.BombMarked;
                } 
        }
        /// <summary>Проверка состояния поля.</summary>
        /// <returns> -1: lose, 0: ongoing, 1: win </returns>
        public GameStates GameState()
        {
            // check lose
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (_rev[x, y] == RevealStates.Opened && _field[x, y] == -1)
                        return GameStates.Lose;
            int revealed = 0;
            int mines = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    if (_rev[x, y] == RevealStates.Opened)
                        revealed++;
                    if (_field[x, y] == -1)
                        mines++;
                }
            if (revealed == Width * Height - mines)
                return GameStates.Win;
            return GameStates.Started;
        }
        private bool outOfBounds(int x, int y) => x < 0 || y < 0 || x >= Width || y >= Height;
        private int CalcNear(int x, int y)
        {
            int sum = 0;
            for (int dy = -1; dy <= 1; dy++)
                for (int dx = -1; dx <= 1; dx++)
                    if (!outOfBounds(x + dx, y + dy))
                        sum += _field[x + dx, y + dy] == -1 ? 1 : 0;
            return sum;
        }
        /// <summary>Очистка поля.</summary>
        public void Reset()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _field[x, y] = 0;
                    _rev[x, y] = RevealStates.Closed;
                }
            }
        }
        /// <summary>Генерация поля с определённым количеством мин.</summary>
        public void Generate(int mines, int tile_x, int tile_y)
        {
            if (mines < 1 || mines > Width * Height - 1)
                return;
            var rand = new Random(DateTime.Now.GetHashCode());
            // place mines
            while (mines > 0)
            {
                int x = rand.Next(Width);
                int y = rand.Next(Height);
                if (_field[x, y] == -1 || (x == tile_x && y == tile_y))
                    continue;
                _field[x, y] = -1;
                mines--;
            }
            // calc tiles
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    _field[x, y] = _field[x, y] != -1 ? CalcNear(x, y) : -1;
        }
        /// <summary>Вывод поля в консоль.</summary>
        public void ConsoleOutput(bool full = false)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (full || _rev[x, y] == RevealStates.Opened)
                    {
                        if (_field[x, y] == -1)
                            Console.Write("*");
                        else if (_field[x, y] == 0)
                            Console.Write(" ");
                        else
                            Console.Write(_field[x, y]);
                    }
                    else if (_rev[x, y] == 0)
                        Console.Write("#");
                    else if (_rev[x, y] == RevealStates.Flagged)
                        Console.Write("P");
                }
                Console.WriteLine();
            }
        }
        public RevealStates RevealState(int x, int y) => _rev[x, y];
        /// <summary>Раскрытие клетки поля.</summary>
        public void Reveal(int x, int y)
        {
            if (outOfBounds(x, y) || _rev[x, y] != RevealStates.Closed)
                return;
            _rev[x, y] = RevealStates.Opened;
            if (_field[x, y] != 0)
                return;
            for (int dy = -1; dy <= 1; dy++)
                for (int dx = -1; dx <= 1; dx++)
                    Reveal(x + dx, y + dy);
        }
        /// <summary>Установка/снятие флажка.</summary>
        public void SwitchFlag(int x, int y)
        {
            if (!outOfBounds(x, y) && _rev[x, y] != RevealStates.Opened)
                _rev[x, y] = _rev[x, y] == RevealStates.Closed ? RevealStates.Flagged : RevealStates.Closed;
        }
    }
}