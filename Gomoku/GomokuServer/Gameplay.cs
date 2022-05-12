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

        public void CellIsBusy(Point point)
        {
            if (_field[point.X, point.Y] != Cell.Empty)
                throw new ApplicationException("Cell is busy");
        }

        public Cell this[int x, int y]
        {
            get
            {
                if (x < 0 || x > 15 || y < 0 || y > 15)
                    return Cell.Empty;
                return _field[x, y];
            }
        }

        public List<Point> CheckField(Cell player, int kx, int ky)
        {
            List<Point> Win = new List<Point>();
            Win.Add(new Point() { X = _point.X, Y = _point.Y });

            for (var i = 0; i < 5; ++i)
            {
                if (_field[_point.X + i * kx, _point.Y + i * ky] != player)
                    break;
                else
                    Win.Add(new Point() { X = _point.X + i * kx, Y = _point.Y + i * ky });
            }

            if (kx == 1)
                kx = -1;
            if (kx == -1)
                kx = 1;
            if (ky == 1)
                ky = -1;
            if (ky == -1)
                ky = 1;

            for (var i = 0; i < 5; ++i)
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
            for (var i = 0; i < _field.Length; ++i)
                if (_field[_point.X + i, _point.Y + i] == Cell.Empty)
                    return;
            _winner = Cell.Empty;
        }

        public bool GameCheck()
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
