namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers;

public class SegmentedDragger<T> : ILocalDragger<T> where T : struct
{
    public void NewPoint(MouseMessageType type, T point)
    {
        PreAllPoints(type, point);
        SendClickSpecificMessage(type, point);
        PostAllPoints(type, point);
    }

    private void SendClickSpecificMessage(MouseMessageType type, T point)
    {
        switch (type)
        {
            case MouseMessageType.Down:
                MouseDown(point);
                break;
            case MouseMessageType.Move:
                MouseMove(point);
                break;
            case MouseMessageType.Up:
                MouseUp(point);
                break;
        }
    }

    protected virtual void MouseDown(T point) {}
    protected virtual void MouseMove(T point) {}
    protected virtual void MouseUp(T point) {}
    protected virtual void PreAllPoints(MouseMessageType type, T point){}
    public virtual void PostAllPoints(MouseMessageType type, T point){}
}