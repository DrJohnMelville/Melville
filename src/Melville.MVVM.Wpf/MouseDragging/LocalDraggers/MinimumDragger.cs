using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers
{
    public class MinimumDragger : SegmentedDragger<Point>
    {
        private readonly double radiusSquared;
        private readonly ILocalDragger<Point> effector;
        private Point initialPoint;
        private bool hasEscapedBefore;

        public MinimumDragger(double radius, ILocalDragger<Point> effector)
        {
            radiusSquared = radius * radius;
            this.effector = effector;
        }

        protected override void MouseDown(Point point) => initialPoint = point;

        public override void PostAllPoints(MouseMessageType type, Point point)
        {
            if (HasEscapedInitialRadius(point))
                effector.NewPoint(type, point);
        }

        private bool HasEscapedInitialRadius(Point point)
        {
            if (hasEscapedBefore) return true;
            if (IsInsideDelta(initialPoint - point)) return false;
            OnFirstEscapeFromRadius();
            return true;
        }

        private bool IsInsideDelta(Vector delta) => delta.LengthSquared < radiusSquared;

        private void OnFirstEscapeFromRadius()
        {
            hasEscapedBefore = true;
            effector.NewPoint(MouseMessageType.Down, initialPoint); // now we send the original point to start the drag
        }
    }
}