using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers
{
    public class MinimumDragger : ILocalDragger<Point>
    {
        private readonly double radiusSquared;
        private readonly ILocalDragger<Point> effector;
        private Point initialPoint;
        private bool triggered;

        public MinimumDragger(double radius, ILocalDragger<Point> effector)
        {
            radiusSquared = radius * radius;
            this.effector = effector;
        }

        public void NewPoint(MouseMessageType type, Point point)
        {
            if (type == MouseMessageType.Down)
            {
                initialPoint = point;
                return;
            }

            if (triggered || CheckStillInsideDelta(point)) return;
            effector.NewPoint(type, point);
        }

        private bool CheckStillInsideDelta(Point point)
        {
            if (IsInsideDelta(initialPoint - point)) return true;
            triggered = true;
            effector.NewPoint(MouseMessageType.Down, initialPoint);
            return false;
        }
        private bool IsInsideDelta(Vector delta) => delta.LengthSquared < radiusSquared;
    }
}