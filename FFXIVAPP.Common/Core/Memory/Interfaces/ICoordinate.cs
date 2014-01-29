// FFXIVAPP.Common
// ICoordinate.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Memory.Interfaces
{
    public interface ICoordinate
    {
        double X { get; set; }
        double Z { get; set; }
        double Y { get; set; }
        Coordinate Rotate2D(float angle);
        Coordinate Subtract(Coordinate coordinate);
        Coordinate Add(Coordinate coordinate);
        Coordinate Add(float x, float y, float z);
        Coordinate Scale(float scale);
        Coordinate Normalize();
        Coordinate Normalize(Coordinate origin);
        float AngleTo(Coordinate coordinate);
        float DistanceTo(Coordinate coordinate);
        float Distance2D(Coordinate coordinate);
    }
}
