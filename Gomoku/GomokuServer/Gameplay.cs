using System;
using System.Collections.Generic;

using Gomoku;

using static GomokuServer.FieldExtensions;

namespace GomokuServer
{
    public class Gameplay
    {
        public Cell? _winner = null;

        public FieldExtensions _gameField = new FieldExtensions();

        public List<Point> _winPoints = new List<Point>();

        public Point _point = new Point { X = 0, Y = 0 };

        public void EnterIntoTheCell(Point point, bool _isFirstTurn)
        {
            _gameField._field[point.X, point.Y] =
                _isFirstTurn
                ? Cell.FirstPlayer
                : Cell.SecondPlayer;

            _point = point;
        }

        public void CellIsBusy(Point point)
        {
            if (_gameField[point.X, point.Y] != Cell.Empty)
                throw new ApplicationException("Cell is busy");
        }

        public List<Point> CheckField(Cell player, int kx, int ky)
        {
            List<Point> Win = new List<Point>();

            for (var i = 0; i < 5; ++i)
            {
                if (_gameField[_point.X + i * kx, _point.Y + i * ky] != player)
                    break;
                else
                    Win.Add(new Point() { X = _point.X + i * kx, Y = _point.Y + i * ky });
            }

            kx *= -1;
            ky *= -1;

            for (var i = 1; i < 5; ++i)
            {
                if (_gameField[_point.X + i * kx, _point.Y + i * ky] != player)
                    break;
                else
                    Win.Add(new Point() { X = _point.X + i * kx, Y = _point.Y + i * ky });
            }

            if (Win.Count < 5)
                return new List<Point>();
            _winner = player;
            return Win;
        }

        public void CheckDraw()
        {
            for (var i = 0; i < 15; ++i)
                for (var j = 0; j < 15; ++j)
                    if (_gameField[i, j] == Cell.Empty)
                        return;
            _winner = Cell.Empty;
        }

        public bool CheckGame()
        {
            var player = _gameField[_point.X, _point.Y];

            CheckDraw();
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
