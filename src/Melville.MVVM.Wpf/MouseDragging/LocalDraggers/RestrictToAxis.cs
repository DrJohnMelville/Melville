using System;
using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers
{
    public class RestrictToAxis : ILocalDragger<Point>
    {
        private Point initialPoint;
        private readonly ILocalDragger<Point> target;

        public RestrictToAxis(ILocalDragger<Point> target)
        {
            this.target = target;
        }

        public void NewPoint(MouseMessageType type, Point point)
        {
            TryRecordInitialPoint(type, point);
            target.NewPoint(type, AdjustPoint(point));
        }

        private void TryRecordInitialPoint(MouseMessageType type, Point point)
        {
            if (type == MouseMessageType.Down) initialPoint = point;
        }

        private Point AdjustPoint(Point point)
        {
            var delta = point - initialPoint;
            return Math.Abs(delta.X) > Math.Abs(delta.Y)
                ? new Point(point.X, initialPoint.Y)
                : new Point(initialPoint.X, point.Y);
        }
    }
}