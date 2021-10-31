using System;
using System.Linq;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers;

public class CircleSnapper: ILocalDragger<CircularPoint>
{
    private readonly double[] anchors;
    private readonly double halfWidth;
    private readonly ILocalDragger<CircularPoint> target;

    public CircleSnapper(int numPoints, double width, ILocalDragger<CircularPoint> target)
    {
        this.target = target;
        anchors = ComputeAnchors(numPoints);
        halfWidth = width / 2;
    }

    private static double[] ComputeAnchors(int numPoints) => 
        Enumerable.Range(0, numPoints + 1).Select(i => i * 360.0 / numPoints).ToArray();

    public void NewPoint(MouseMessageType type, CircularPoint point) => 
        target.NewPoint(type, new CircularPoint(Snap(point.AngleInDegrees, type), point.Length));

    protected virtual double Snap(double angleInDegrees, MouseMessageType type)
    {
        foreach (var anchor in anchors)
        {
            if (IsCloseEnoughToSnapToAnchor(angleInDegrees, anchor)) return anchor;
        }
        return angleInDegrees;
    }

    private bool IsCloseEnoughToSnapToAnchor(double angleInDegrees, double anchor) => 
        Math.Abs(anchor - angleInDegrees) < halfWidth;
}

public class MouseUpCircleSnapper : CircleSnapper
{
    public MouseUpCircleSnapper(int numPoints, double width, ILocalDragger<CircularPoint> target) :
        base(numPoints, width, target)
    {
    }

    protected override double Snap(double angleInDegrees, MouseMessageType type) => 
        type == MouseMessageType.Up ? base.Snap(angleInDegrees, type): angleInDegrees;
}