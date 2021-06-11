using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers
{
    public class MinimumDragger : SegmentedDragger<Point>
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
        
        protected override void MouseDown(Point point) => initialPoint = point;

        public override void PostAllPoints(MouseMessageType type, Point point)
        {
            if (HasEscapedInitialRadius(point)) 
                effector.NewPoint(type, point);
        }

        private bool HasEscapedInitialRadius(Point point) => triggered || CheckStillInsideDelta(point);

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