using System;
using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers
{
    public class InitialPointDragger : SegmentedDragger<Point>
    {
        private readonly Point origin;
        private readonly ILocalDragger<Point> target;
        private Vector offset;

        public InitialPointDragger(Point origin, ILocalDragger<Point> target)
        {
            this.origin = origin;
            this.target = target;
        }

        protected override void MouseDown(Point point) => offset = origin - point;

        public override void PostAllPoints(MouseMessageType type, Point point) => 
            target.NewPoint(type, point + offset);
    }
}