using System.Linq;
using System.Windows;
using System.Windows.Input;
using Melville.MVVM.Wpf.MouseDragging.Drag;
using Melville.MVVM.Wpf.WpfHacks;

namespace Melville.MVVM.Wpf.MouseDragging
{
  public sealed class WindowMouseDataSource : MouseDataSource
  {
    public FrameworkElement Target { get; }

    public WindowMouseDataSource(FrameworkElement target, MouseButtonEventArgs args)
    {
      Target = target;
      if (args != null)
      {
        BindToPhysicalMouse(args);
      }
    }

    // this is a test hook
    public override void SendMousePosition(MouseMessageType type, Point position)
    {
      CheckInitialArgsSent();
      base.SendMousePosition(type, position);
    }

    protected override LocalDragEventArgs CreateLocalDragEventArgs(MouseMessageType type, Point position)
    {
      return new LocalDragEventArgs(position, type, new Size(Target.ActualWidth, Target.ActualHeight), 
        buttonBeingDragged, Target);
    }

    private MouseButton buttonBeingDragged;

    private MouseButtonEventArgs? initialArgs;
    private IInputElement? topElement;
    private void BindToPhysicalMouse(MouseButtonEventArgs args)
    {
      topElement = Target.Parents().OfType<IInputElement>().Last();
      topElement.CaptureMouse();
      buttonBeingDragged = args.ChangedButton;
      topElement.MouseMove += OnMouseMoved;
      if (topElement is DependencyObject moveSource)
      {
        Mouse.AddMouseUpHandler(moveSource, MouseUp);
      }
      initialArgs = args;
    }

    private void SendMousePosition(MouseMessageType type, MouseEventArgs args)
    {
      SendMousePosition(type, args.GetPosition(Target));
    }

    private void CheckInitialArgsSent()
    {
      if (initialArgs == null) return;
      var capture = initialArgs;
      initialArgs = null;
      SendMousePosition(MouseMessageType.Down, capture);
    }

    private void MouseUp(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != buttonBeingDragged) return;
      e.Handled = true;
      SendMousePosition(MouseMessageType.Up, e);
      CancelMouseBinding();
    }

    public override void CancelMouseBinding()
    {
      if (topElement == null) return;
      topElement.MouseMove -= OnMouseMoved;
      if (topElement is DependencyObject moveSource)
      {
        Mouse.RemoveMouseUpHandler(moveSource, MouseUp);
      }

      topElement.ReleaseMouseCapture();
    }

    public void OnMouseMoved(object sender, MouseEventArgs e)
    {
      SendMousePosition(MouseMessageType.Move, e);
      e.Handled = true;
    }

    public override DragDropEffects InitiateDrag(IDataObject draggedData, DragDropEffects allowedEffects,
      LocalDragEventArgs args) =>  new DragHandler(this, args).InitiateDrag(draggedData, allowedEffects);

    public override IDragUIWindow ConstructDragWindow(FrameworkElement target, double opacity) => 
      new DragUIWindow(target, opacity);
  }
}