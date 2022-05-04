using System;

namespace GeoApp.Model
{
    public class Coords
    {
        public double x { get; set; }
        public double y { get; set; }

        public Coords() { }
    }

    public class ATM
    {
        public string Id { get; set; }
        public string BankName { get; set; }
        public int Balance { get; set; }
        public Coords Coords { get; set; }
    }
}
