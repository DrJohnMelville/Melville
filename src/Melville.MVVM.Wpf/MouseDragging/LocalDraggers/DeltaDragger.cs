using System;
using System.Windows;
using Melville.Hacks.Reflection;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers
{
    public class DeltaDragger : ILocalDragger<Point>
    {
        private readonly ILocalDragger<Point> target;
        private Point lastPoint;

        public DeltaDragger(ILocalDragger<Point> target)
        {
            this.target = target;
        }

        public void NewPoint(MouseMessageType type, Point point)
        {
            RecordLastPointIfDownMessage(type, point);
            NotifyDeltaMove(type, point);
            RecordLastPoint(point);
        }

        private void NotifyDeltaMove(MouseMessageType type, Point point) => 
            target.NewPoint(type, (point - lastPoint).AsPoint());

        private void RecordLastPointIfDownMessage(MouseMessageType type, Point point)
        {
            if (type == MouseMessageType.Down) RecordLastPoint(point);
        }

        private void RecordLastPoint(Point point) => lastPoint = point;
    }

    public static class VectorOperations
    {
        public static Point AsPoint(this Vector vec) => new Point(vec.X, vec.Y);
    }
}