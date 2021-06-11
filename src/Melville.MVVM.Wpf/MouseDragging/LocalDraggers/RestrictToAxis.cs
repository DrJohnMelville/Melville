using System;
using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers
{
    public class RestrictToAxis : SegmentedDragger<Point>
    {
        private Point initialPoint;
        private readonly ILocalDragger<Point> target;

        public RestrictToAxis(ILocalDragger<Point> target)
        {
            this.target = target;
        }

        protected override void MouseDown(Point point) => initialPoint = point;

        public override void PostAllPoints(MouseMessageType type, Point point) => 
            target.NewPoint(type, AdjustPoint(point));

        private Point AdjustPoint(Point point)
        {
            var delta = point - initialPoint;
            return Math.Abs(delta.X) > Math.Abs(delta.Y)
                ? new Point(point.X, initialPoint.Y)
                : new Point(initialPoint.X, point.Y);
        }
    }
}