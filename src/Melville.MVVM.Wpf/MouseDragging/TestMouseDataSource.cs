using  System;
using System.Windows;
using System.Windows.Input;
using Melville.MVVM.Wpf.MouseDragging.Drag;

namespace Melville.MVVM.Wpf.MouseDragging
{
  public sealed class TestMouseDataSource : MouseDataSource
  {

    private readonly MouseButton buttonUsed;
    private readonly Size windowSize;
    private readonly FrameworkElement target;

    public TestMouseDataSource(MouseButton buttonUsed = MouseButton.Left, double width = 100, double height = 100,
      FrameworkElement? target = null)
    {
      this.buttonUsed = buttonUsed;
      this.windowSize = new Size(width, height);
      this.target = target!; // this is ok because it is only a test class -- should not be in  production code
    }

    public override void CancelMouseBinding()
    {
    }

    protected override LocalDragEventArgs CreateLocalDragEventArgs(MouseMessageType type, Point position) =>
      new LocalDragEventArgs(position, type, windowSize, buttonUsed, target);

    public IDataObject? DataObject { get; private set; }
    public LocalDragEventArgs? DragArgs { get; private set; }
    public DragDropEffects EffectsIn { get; private set; }
    public DragDropEffects EffectsOut { get; set; }

    public override DragDropEffects InitiateDrag(IDataObject draggedData, DragDropEffects allowedEffects,
      LocalDragEventArgs args)
    {
      DataObject = draggedData;
      DragArgs = args;
      EffectsIn = allowedEffects;
      return EffectsOut;
    }

    #region Wrapper

    public IMouseDragger WrapWithMouseDragger(MouseButton draggedButton = MouseButton.Left, int initialClickCount = 1) =>
      new draggerImplementation(this, draggedButton, initialClickCount);

    private class draggerImplementation : IMouseDragger
    {
      private TestMouseDataSource source;

      public draggerImplementation(TestMouseDataSource source, MouseButton draggedButton, int initialClickCount)
      {
        this.source = source;
        DraggedButton = draggedButton;
        InitialClickCount = initialClickCount;
      }

      public IMouseDataSource LeafTarget() => source;

      public IMouseDataSource NamedTarget(string name) => source;

      public IMouseDataSource TypedTarget<T>() where T : DependencyObject => source;

      public IMouseDataSource TopTarget() => source;

      public MouseButton DraggedButton { get; }
      public int InitialClickCount { get; }
    }

    #endregion

    #region DragWindow
    public DragWindowMock? DragWindow { get; private set; }
    public override IDragUIWindow ConstructDragWindow(FrameworkElement target, double opacity)
    {
      return DragWindow = new DragWindowMock(target, opacity);
    }

    public class DragWindowMock : IDragUIWindow
    {
      public FrameworkElement Target { get; }
      public double Opacity { get; }
      public LocalDragEventArgs? LastArgs { get; private set; }

      public DragWindowMock(FrameworkElement target, double opacity)
      {
        Target = target;
        Opacity = opacity;
      }

      public void MouseMoved(LocalDragEventArgs e)
      {
        LastArgs = e;
      }

      public void Show()
      {
      }
    }
    #endregion
  }
}