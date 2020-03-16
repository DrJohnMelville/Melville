using  System;

namespace Melville.MVVM.Wpf.MouseDragging
{
  public class InitialDeltaSource:IMouseDataSource
  {
    private readonly double xTol;
    private readonly double yTol;
    private readonly IMouseDataSource source;

    public InitialDeltaSource(IMouseDataSource source, double xTol, double yTol)
    {
      this.xTol = xTol;
      this.yTol = yTol;
      this.source = source;
      source.MouseMoved += OnMouseMoved;
    }

    private LocalDragEventArgs? mouseDownEventPendingDragActivation = null;
    private void OnMouseMoved(object? sender, LocalDragEventArgs e)
    {
      if (TryHandleIsMouseDownEvent(e)) return;
      TryHandleDragDeltaRange(sender, e);
      SendEvent(sender, e);
    }

    private void TryHandleDragDeltaRange(object? sender, LocalDragEventArgs e)
    {
      if (mouseDownEventPendingDragActivation == null) return;
      var offset = mouseDownEventPendingDragActivation.TransformedPoint - e.TransformedPoint;
      if (Math.Abs(offset.X) >= xTol || Math.Abs(offset.Y) >= yTol)
      {
        SendEvent(sender, mouseDownEventPendingDragActivation);
        mouseDownEventPendingDragActivation = null;
      }
    }

    private bool TryHandleIsMouseDownEvent(LocalDragEventArgs e)
    {
      if (e.MessageType == MouseMessageType.Down)
      {
        mouseDownEventPendingDragActivation = e;
        return true;
      }

      return false;
    }

    private void SendEvent(object? sender, LocalDragEventArgs e)
    {
      MouseMoved?.Invoke(sender, e);
    }

    public IMouseDataRoot Root => source.Root;
    public event EventHandler<LocalDragEventArgs>? MouseMoved;
  }
}