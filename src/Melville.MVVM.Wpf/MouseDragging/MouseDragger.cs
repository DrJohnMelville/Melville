using  System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Melville.MVVM.Wpf.WpfHacks;

namespace Melville.MVVM.Wpf.MouseDragging
{

  public interface IMouseDragger
  {
    IMouseDataSource LeafTarget();
    IMouseDataSource NamedTarget(string name);
    IMouseDataSource TypedTarget<T>() where T : DependencyObject;
    IMouseDataSource TopTarget();

    MouseButton DraggedButton { get; }
    int InitialClickCount { get; }
  }


  public sealed class MouseDragger : IMouseDragger
  {
    private readonly DependencyObject root;
    private readonly MouseButtonEventArgs initalArgs;

    public MouseDragger(DependencyObject root, MouseButtonEventArgs initalArgs)
    {
      this.root = root;
      this.initalArgs = initalArgs;
    }

    public IMouseDataSource LeafTarget() => AttachToTarget(root);

    public IMouseDataSource NamedTarget(string name) => AttachToTarget(root.Parents()
      .FirstOrDefault(j =>
        name.Equals(j.GetValue(FrameworkElement.NameProperty)?.ToString(), StringComparison.Ordinal)));

    public IMouseDataSource TypedTarget<T>() where T : DependencyObject =>
      AttachToTarget(root.Parents().OfType<T>().FirstOrDefault());

    public IMouseDataSource TypedTarget(params Type[] typeOptions) =>
      AttachToTarget(root.Parents().FirstOrDefault(i => typeOptions.Any(j => j.IsInstanceOfType(i))));

    public IMouseDataSource TopTarget() => AttachToTarget(root.Parents().LastOrDefault());

    private IMouseDataSource AttachToTarget(DependencyObject? target) =>
      target is FrameworkElement fe
        ? new WindowMouseDataSource(fe as FrameworkElement, initalArgs)
        : new TestMouseDataSource();

    public MouseButton DraggedButton => initalArgs.ChangedButton;
    public int InitialClickCount => initalArgs.ClickCount;
  }

}