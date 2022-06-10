using System;
using System.Windows;
using Melville.Hacks.Reflection;
using Melville.INPC;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers;

public partial class DeltaDragger : SegmentedDragger<Point>
{
    [FromConstructor]private readonly ILocalDragger<Point> target;
    private Point lastPoint;

    protected override void MouseDown(Point point)
    {
        RecordLastPoint(point);
    }

    public override void PostAllPoints(MouseMessageType type, Point point)
    {
        NotifyDeltaMove(type, point);
        RecordLastPoint(point);
    }
    private void NotifyDeltaMove(MouseMessageType type, Point point) => 
        target.NewPoint(type, (point - lastPoint).AsPoint());

    private void RecordLastPoint(Point point) => lastPoint = point;
}

public static class VectorOperations
{
    public static Point AsPoint(this Vector vec) => new Point(vec.X, vec.Y);
}