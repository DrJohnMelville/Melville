using System;
using System.Windows;
using Melville.INPC;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers;

public partial class InitialPointDragger : SegmentedDragger<Point>
{
    [FromConstructor]private readonly Point origin;
    [FromConstructor]private readonly ILocalDragger<Point> target;
    private Vector offset;

    protected override void MouseDown(Point point) => offset = origin - point;

    public override void PostAllPoints(MouseMessageType type, Point point) => 
        target.NewPoint(type, point + offset);
}