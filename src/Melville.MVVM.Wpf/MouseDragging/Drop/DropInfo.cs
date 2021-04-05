using System.Linq;
using System.Windows;
using Melville.MVVM.Wpf.MouseDragging.Adorners;

namespace Melville.MVVM.Wpf.MouseDragging.Drop
{
  public interface IDropInfo
  {
    IDataObject Item { get; }
    Point GetTargetLocation();
    Point GetRelativeTargetLocation();
  }

  public interface IDropQuery : IDropInfo
  {
    void AdornTarget(DropAdornerKind kind);
    void AdornBounds(Rect bounds);
  }

  public interface IDropAction : IDropInfo
  {
    /// <summary>
    /// For dropping into a list, this will return 1 if the drop should be after the present item, or 0 if it should be before the present item.
    /// This method looks at the adorner on that target to decide if it is in  a horizontal or vertical list.
    /// </summary>
    /// <returns>0 if the drop should be before the target element, or 1 if it should be after the target element</returns>
    int InsertAfterItemAdjustment();

    /// <summary>
    /// Determine it the target is adorned with a box adorner.
    /// </summary>
    /// <returns>True  if the target has a box adorner, false otherwise</returns>
    bool HasBoxAdorner();
  }

  public abstract class DropInfo:IDropInfo
  {
    public DropInfo(FrameworkElement target, DragEventArgs eventArgs)
    {
      Target = target;
      this.eventArgs = eventArgs;
    }

    public IDataObject Item => eventArgs.Data;
    public FrameworkElement Target { get; }
    public DragEventArgs eventArgs;

    public Point GetTargetLocation() => eventArgs.GetPosition(Target);

    public Point GetRelativeTargetLocation()
    {
      var rawPoint = GetTargetLocation();
      return new Point(rawPoint.X / Target.ActualWidth, rawPoint.Y/Target.ActualHeight);
    }
  }

  public static class DropInfoOperations
  {
    public static void AdornLeftRight(this IDropQuery query) =>
      query.AdornTarget(query.GetRelativeTargetLocation().X < 0.5 ? DropAdornerKind.Left : DropAdornerKind.Right);
    public static void AdornTopBottom(this IDropQuery query) =>
      query.AdornTarget(query.GetRelativeTargetLocation().Y < 0.5 ? DropAdornerKind.Top : DropAdornerKind.Bottom);

    public static void AdornTopBottomMiddle(this IDropQuery query) => query.AdornTarget(
      Select3Way(query.GetRelativeTargetLocation().Y, DropAdornerKind.Top, DropAdornerKind.Bottom));
    public static void AdornLeftRightMiddle(this IDropQuery query) => query.AdornTarget(
      Select3Way(query.GetRelativeTargetLocation().X, DropAdornerKind.Left, DropAdornerKind.Right));

    private static DropAdornerKind Select3Way(double d, DropAdornerKind preAdorner, DropAdornerKind postAdorner)
    {
      if (d < 0.25) return preAdorner;
      if (d > 0.75) return postAdorner;
      return DropAdornerKind.Rectangle;
    }

  }

  public sealed class DropQuery : DropInfo, IDropQuery
  {
    public DropQuery(DragEventArgs item, FrameworkElement target) : base(target, item)
    {
    }

    public void AdornTarget(DropAdornerKind kind) => Target.Adorn(kind);
    public void AdornBounds(Rect bounds) => Target.Adorn(new RectangleAdorner(Target, bounds));
  }

  public sealed class DropAction : DropInfo, IDropAction
  {
    public DropAction(DragEventArgs item, FrameworkElement target) : base(target, item)
    {

    }

    public int InsertAfterItemAdjustment() => 
      (Target.GetAdorners().Any(i => i is RightAdorner || i is BottomAdorner)) ? 1 : 0;

    public bool HasBoxAdorner() => Target.GetAdorners().OfType<OutlineAdorner>().Any();
  }
}
