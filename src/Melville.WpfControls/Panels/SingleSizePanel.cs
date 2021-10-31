using  System.Windows;

namespace Melville.WpfControls.Panels;

public class SingleSizePanel : TestablePanel
{
  // This is used to offset the toolGrid when the thumbnail has black bars on it.
  public static readonly DependencyProperty OffsetProperty =
    DependencyProperty.Register("Offset", typeof(Thickness), typeof(SingleSizePanel),
      new PropertyMetadata(new Thickness(0, 0, 1, 1)));
  public Thickness Offset
  {
    get { return (Thickness)GetValue(OffsetProperty); }
    set { SetValue(OffsetProperty, value); }
  }


  public bool ApplyOffset
  {
    get { return (bool)GetValue(ApplyOffsetProperty); }
    set { SetValue(ApplyOffsetProperty, value); }
  }

  // Using a DependencyProperty as the backing store for ApplyOffset.  This enables animation, styling, binding, etc...
  public static readonly DependencyProperty ApplyOffsetProperty =
    DependencyProperty.Register("ApplyOffset", typeof(bool), typeof(SingleSizePanel), new PropertyMetadata(false));

  public override Size MeasureOverride(IPanelAdapter adapter, Size availaibleSize)
  {
    if (adapter.ChildrenCount < 1) return new Size(1, 1);

    // measure the first Child
    adapter.Measure(0, availaibleSize);
    var firstSize = adapter.GetDesiredSize(0);

    for (int i = 1; i < adapter.ChildrenCount; i++)
    {
      adapter.Measure(i, firstSize);
    }
    return firstSize;
  }

  public override Size ArrangeOverride(IPanelAdapter adapter, Size finalSize)
  {
    if (adapter.ChildrenCount < 1) return finalSize;
    var desiredSize = adapter.GetDesiredSize(0);

    var finalRect = new Rect(
      new Point((finalSize.Width - desiredSize.Width) / 2, (finalSize.Height - desiredSize.Height) / 2), desiredSize);

    var adjustedRect = ApplyOffset ? new Rect(OffsetPoint(finalRect, Offset.Left, Offset.Top),
      OffsetPoint(finalRect, Offset.Right, Offset.Bottom)) : finalRect;

    for (int i = 0; i < adapter.ChildrenCount; i++)
    {
      adapter.Arrange(i, i == 0 ? finalRect : adjustedRect);
    }
    return finalSize;
  }

  private Point OffsetPoint(Rect finalRect, double xOffset, double yOffset)
  {
    return new Point(finalRect.X + (finalRect.Width * xOffset),
      finalRect.Y + (finalRect.Height * yOffset));
  }
}