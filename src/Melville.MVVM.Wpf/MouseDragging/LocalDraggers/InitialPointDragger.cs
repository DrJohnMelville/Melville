using System;
using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers
{
    public class InitialPointDragger : ILocalDragger<Point>
    {
        private readonly Point origin;
        private readonly ILocalDragger<Point> target;
        private Vector offset;

        public InitialPointDragger(Point origin, ILocalDragger<Point> target)
        {
            this.origin = origin;
            this.target = target;
        }

        public void NewPoint(MouseMessageType type, Point point)
        {
            if (type == MouseMessageType.Down)
            {
                offset = origin - point;
            }
            target.NewPoint(type, point + offset);
        }
    }
}