namespace GomokuServer
{
    public class Playground
    {
        private readonly Cell[,] _playground = new Cell[15, 15];

        public Cell this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= 15 || y < 0 || y >= 15)
                    return Cell.Empty;
                return _playground[x, y];
            }

            set
            {
                _playground[x, y] = value;
            }
        }

    }
}
