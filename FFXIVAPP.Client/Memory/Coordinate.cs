// FFXIVAPP.Client
// Coordinate.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Client.Memory {
    public class Coordinate {
        public double X;
        public double Y;
        public double Z;

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Coordinate(double x = 0, double y = 0, double z = 0.00) {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return X + ", " + Y + ", " + Z;
        }
    }
}
