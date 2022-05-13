using System;
using System.Collections.Generic;

using Gomoku;

namespace GomokuServer
{
    public class Gameplay
    {
        public enum Cell
        {
            Empty = 0,
            FirstPlayer,
            SecondPlayer
        }

        public readonly Cell[,] _field = new Cell[15, 15];

        public Cell? _winner = null;

        public List<Point> _winPoints = new List<Point>();

        public Point _point = new Point { X = 0, Y = 0 };

        public void EnterIntoTheCell(Point point, bool _isFirstTurn)
        {
            _field[point.X, point.Y] =
                _isFirstTurn
                ? Cell.FirstPlayer
                : Cell.SecondPlayer;

            _point = point;
        }

        public bool CellIsBusy(Point point)
        {
            return (_field[point.X, point.Y] != Cell.Empty);
        }

        public Cell this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= 15 || y < 0 || y >= 15)
                    return Cell.Empty;
                return _field[x, y];
            }
        }

        private int ChangeCoef(int coef)
        {
            if (coef == 1)
                return -1;
            else
                if (coef == -1)
                    return 1;
            return 0;
        }

        public List<Point> CheckField(Cell player, int kx, int ky)
        {
            List<Point> Win = new List<Point>();

            for (var i = 0; i < 5; ++i)
            {
                if (_field[_point.X + i * kx, _point.Y + i * ky] != player)
                    break;
                else
                    Win.Add(new Point() { X = _point.X + i * kx, Y = _point.Y + i * ky });
            }

            kx = ChangeCoef(kx);
            ky = ChangeCoef(ky);

            for (var i = 1; i < 5; ++i)
            {
                if (_field[_point.X + i * kx, _point.Y + i * ky] != player)
                    break;
                else
                    Win.Add(new Point() { X = _point.X + i * kx, Y = _point.Y + i * ky });
            }

            if (Win.Count < 5)
                return new List<Point>();
            _winner = player;
            return Win;
        }

        public void CheckDefeat()
        {
            for (var i = 0; i < 15; ++i)
                for (var j = 0; j < 15; ++j)
                    if (_field[i,j] == Cell.Empty)
                        return;
            _winner = Cell.Empty;
        }

        public bool CheckGame()
        {
            var player = _field[_point.X, _point.Y];

            CheckDefeat();
            if (_winner == Cell.Empty)
                return true;

            var points = CheckField(player, 1, 0);
            if (points.Count != 0)
            {
                _winPoints = points;
                return true;
            }

            points = CheckField(player, 0, 1);
            if (points.Count != 0)
            {
                _winPoints = points;
                return true;
            }

            points = CheckField(player, -1, 1);
            if (points.Count != 0)
            {
                _winPoints = points;
                return true;
            }

            points = CheckField(player, 1, 1);
            if (points.Count != 0)
            {
                _winPoints = points;
                return true;
            }

            return false;
        }
    }
}
