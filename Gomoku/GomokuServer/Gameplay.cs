using System;
using System.Collections.Generic;

using Gomoku;

namespace GomokuServer
{
    public class Gameplay
    {
        public Cell? Winner { get; set; } = null;

        public Playground GameField { get; } = new();

        public List<Point> WinPoints { get; } = new();

        public Point PlayerPoint { get; private set; } = new() { X = 0, Y = 0 };

        public Gameplay(Playground playground)
        {
            GameField = playground;
        }

        public void EnterIntoTheCell(Point point, bool isFirstTurn)
        {
            GameField[point.X, point.Y] =
                isFirstTurn
                ? Cell.FirstPlayer
                : Cell.SecondPlayer;

            PlayerPoint = point;
        }

        public void CellIsBusy(Point point)
        {
            if (GameField[point.X, point.Y] != Cell.Empty)
                throw new ApplicationException("Cell is busy");
        }

        public List<Point> CheckField(Cell player, int kx, int ky)
        {
            List<Point> Win = new List<Point>();

            for (var i = 0; i < 5; ++i)
            {
                if (GameField[PlayerPoint.X + i * kx, PlayerPoint.Y + i * ky] != player)
                    break;
                else
                    Win.Add(new Point() { X = PlayerPoint.X + i * kx, Y = PlayerPoint.Y + i * ky });
            }

            kx *= -1;
            ky *= -1;

            for (var i = 1; i < 5; ++i)
            {
                if (GameField[PlayerPoint.X + i * kx, PlayerPoint.Y + i * ky] != player)
                    break;
                else
                    Win.Add(new Point() { X = PlayerPoint.X + i * kx, Y = PlayerPoint.Y + i * ky });
            }

            if (Win.Count < 5)
                return new List<Point>();
            Winner = player;
            return Win;
        }

        public void CheckDraw()
        {
            for (var i = 0; i < 15; ++i)
                for (var j = 0; j < 15; ++j)
                    if (GameField[i, j] == Cell.Empty)
                        return;
            Winner = Cell.Empty;
        }

        public bool CheckGame()
        {
            var player = GameField[PlayerPoint.X, PlayerPoint.Y];

            CheckDraw();
            if (Winner == Cell.Empty)
                return true;

            var points = CheckField(player, 1, 0);
            if (points.Count != 0)
            {
                WinPoints.AddRange(points);
                return true;
            }

            points = CheckField(player, 0, 1);
            if (points.Count != 0)
            {
                WinPoints.AddRange(points);
                return true;
            }

            points = CheckField(player, -1, 1);
            if (points.Count != 0)
            {
                WinPoints.AddRange(points);
                return true;
            }

            points = CheckField(player, 1, 1);
            if (points.Count != 0)
            {
                WinPoints.AddRange(points);
                return true;
            }

            return false;
        }
    }
}
