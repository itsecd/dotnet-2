using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuServer.Models
{
    public class Field
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public List<Point> Points { get; set; } = new List<Point>();

        public int this[int x, int y]
        {
            get
            {
                var point = Points.FirstOrDefault(p => p.X == x && p.Y == y);
                if (point is null)
                    return -1;
                return point.Value;
            }
            set
            {
                var point = Points.FirstOrDefault(p => p.X == x && p.Y == y);
                if (point is null)
                    Points.Add(new Point() { X = x, Y = y, Value = value });
                else
                    point.Value = value;
            }
        }
    }
}
