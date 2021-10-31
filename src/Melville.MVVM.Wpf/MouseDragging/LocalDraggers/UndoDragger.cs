using System.Windows;
using Melville.MVVM.Undo;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers;

public class UndoDragger<T>: SegmentedDragger<T> where T : struct
{
    private readonly IUndoEngine undo;
    private readonly ILocalDragger<T> effector;
    private T initialPoint;

    public UndoDragger(IUndoEngine undo, ILocalDragger<T> effector)
    {
        this.undo = undo;
        this.effector = effector;
    }

    protected override void MouseDown(T point) => initialPoint = point;

    protected override void MouseUp(T point) =>
        undo.PushWithoutDoing(
            ()=>effector.NewPoint(MouseMessageType.Up, point),
            ()=>effector.NewPoint(MouseMessageType.Up, initialPoint));

    public override void PostAllPoints(MouseMessageType type, T point) => effector.NewPoint(type, point);
}