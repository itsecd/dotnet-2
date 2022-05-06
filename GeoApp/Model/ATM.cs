using System;

namespace GeoApp.Model
{
    public class Coords
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class ATM
    {
        public string Id { get; set; }
        public string BankName { get; set; }
        public int Balance { get; set; }
        public Coords Coords { get; set; }
    }
}
