// FFXIVAPP.Common
// Coordinate.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Memory
{
    public class Coordinate : ICoordinate
    {
        public Coordinate(double x = 0.00, double z = 0.00, double y = 0.00)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; set; }
        public double Z { get; set; }
        public double Y { get; set; }

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }
    }
}
