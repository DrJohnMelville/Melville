using System.Windows;
using Melville.INPC;
using Melville.MVVM.Undo;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers;

public partial class UndoDragger<T>: SegmentedDragger<T> where T : struct
{
    [FromConstructor]private readonly IUndoEngine undo;
    [FromConstructor]private readonly ILocalDragger<T> effector;
    private T initialPoint;

    protected override void MouseDown(T point) => initialPoint = point;

    protected override void MouseUp(T point) =>
        undo.PushWithoutDoing(
            ()=>effector.NewPoint(MouseMessageType.Up, point),
            ()=>effector.NewPoint(MouseMessageType.Up, initialPoint));

    public override void PostAllPoints(MouseMessageType type, T point) => effector.NewPoint(type, point);
}