// FFXIVAPP.Client
// Coordinate.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Client.Memory
{
    public class Coordinate
    {
        public float X;
        public float Y;
        public float Z;

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Coordinate(float x = 0, float y = 0, float z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }
    }
}
