using System;
using System.Collections.Generic;

using Gomoku;

namespace GomokuServer
{
    public class FieldExtensions
    {
        public enum Cell
        {
            Empty = 0,
            FirstPlayer,
            SecondPlayer
        }

        public readonly Cell[,] _field = new Cell[15, 15];

        public Cell this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= 15 || y < 0 || y >= 15)
                    return Cell.Empty;
                return _field[x, y];
            }
        }

    }
}
