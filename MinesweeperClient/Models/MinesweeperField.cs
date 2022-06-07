// 0 пустая клетка
// 1 - 8 цифра
// -1 бомба
using System;

namespace MinesweeperClient.Models
{
    /// <summary>Поле для игры "Сапёр".</summary>
    public class MinesweeperField
    {
        /// <summary>Ширина поля.</summary>
        private int _width;
        /// <summary>Высота поля.</summary>
        private int _height;
        private readonly int[,] _field;
        private readonly TileStates[,] _tiles;
        public int this[int x, int y] => _field[x, y];

        /// <summary>Создание поля определённого размера.</summary>
        public MinesweeperField(int height, int width)
        {
            _height = height;
            _width = width;
            _field = new int[_width, _height];
            _tiles = new TileStates[_width, _height];
        }
        /// <summary>Раскрытие расположения всех мин.</summary>
        public void RevealMines()
        {
            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                {
                    if (_field[x, y] == -1 && _tiles[x, y] == TileStates.Closed)
                        _tiles[x, y] = TileStates.Opened;
                    else if (_field[x, y] == -1 && _tiles[x, y] == TileStates.Flagged)
                        _tiles[x, y] = TileStates.Marked;
                }
        }
        /// <summary>Проверка состояния игры.</summary>
        public GameStatus GameState()
        {
            int revealed = 0;
            int mines = 0;
            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                {
                    if (_tiles[x, y] == TileStates.Opened && _field[x, y] == -1)
                        return GameStatus.Lose;
                    if (_tiles[x, y] == TileStates.Opened)
                        revealed++;
                    if (_field[x, y] == -1)
                        mines++;
                }
            if (revealed == _width * _height - mines)
                return GameStatus.Win;
            if (revealed == 0)
                return GameStatus.Ready;
            return GameStatus.InProgress;
        }
        /// <summary>Проверка выхода за границы поля.</summary>
        private bool OutOfBounds(int x, int y) => x < 0 || y < 0 || x >= _width || y >= _height;
        /// <summary>Расчет количества мин вокруг клетки.</summary>
        private int CalcNear(int x, int y)
        {
            int sum = 0;
            for (int dy = -1; dy <= 1; dy++)
                for (int dx = -1; dx <= 1; dx++)
                    if (!OutOfBounds(x + dx, y + dy))
                        sum += _field[x + dx, y + dy] == -1 ? 1 : 0;
            return sum;
        }
        /// <summary>Очистка поля.</summary>
        public void Reset()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _field[x, y] = 0;
                    _tiles[x, y] = TileStates.Closed;
                }
            }
        }
        /// <summary>Генерация поля с определённым количеством мин.</summary>
        public void Generate(int tileX, int tileY, int mines = 0)
        {
            if (mines < 1 || mines > _width * _height - 1)
                return;
            var rand = new Random(DateTime.Now.GetHashCode());
            while (mines > 0)
            {
                int x = rand.Next(_width);
                int y = rand.Next(_height);
                if (_field[x, y] == -1 || (x == tileX && y == tileY))
                    continue;
                _field[x, y] = -1;
                mines--;
            }
            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                    _field[x, y] = _field[x, y] != -1 ? CalcNear(x, y) : -1;
        }
        /// <summary>Проверка состояния клетки.</summary>
        public TileStates TileState(int x, int y) => _tiles[x, y];
        /// <summary>Раскрытие клетки поля.</summary>
        public void Reveal(int x, int y)
        {
            if (OutOfBounds(x, y) || _tiles[x, y] != TileStates.Closed)
                return;
            _tiles[x, y] = TileStates.Opened;
            if (_field[x, y] != 0)
                return;
            for (int dy = -1; dy <= 1; dy++)
                for (int dx = -1; dx <= 1; dx++)
                    Reveal(x + dx, y + dy);
        }
        /// <summary>Установка/снятие флажка.</summary>
        public void SwitchFlag(int x, int y)
        {
            if (!OutOfBounds(x, y) && _tiles[x, y] != TileStates.Opened)
                _tiles[x, y] = _tiles[x, y] == TileStates.Closed ? TileStates.Flagged : TileStates.Closed;
        }
    }
}