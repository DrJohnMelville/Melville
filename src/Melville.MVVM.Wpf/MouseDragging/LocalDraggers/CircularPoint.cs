using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers
{
    public readonly struct CircularPoint
    {
        public double AngleInDegrees { get; }
        public double Length { get; }

        public CircularPoint(double angleInDegrees, double length)
        {
            AngleInDegrees = angleInDegrees;
            Length = length;
        }

        public static CircularPoint FromVectors(Vector zeroVector, Vector point) =>
            new(AdjustAngle0To360(Vector.AngleBetween(point, zeroVector)), point.Length);

        private static double AdjustAngle0To360(double angleBetween) => 
            (angleBetween < 0)? 360.0+angleBetween:angleBetween;

        public static CircularPoint FromPoints(Point origin, Point end) =>
            FromVectors(new Vector(1, 0), end - origin);
    }
}