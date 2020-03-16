using  System.Windows;
using System.Windows.Controls;

namespace Melville.WpfControls.Panels
{
  public interface IPanelAdapter
  {
    void Measure(int child, Size availableSize);
    Size GetDesiredSize(int child);
    void Arrange(int child, Rect finalRect);
    int ChildrenCount { get; }
    object GetChildProperty(int child, DependencyProperty property);
  }

  public static class IPanelAdaptorMethods
  {
    public static void Measure(this IPanelAdapter adapter, int child, double width, double height) =>
      adapter.Measure(child, new Size(width, height));

    public static void Arrange(this IPanelAdapter adapter, int child, double left, double top, double width,
      double height) =>
      adapter.Arrange(child, new Rect(left, top, width, height));
  }
  public abstract class TestablePanel : Panel, IPanelAdapter
  {
    protected sealed override Size MeasureOverride(Size availableSize) => MeasureOverride(this, availableSize);

    protected sealed override Size ArrangeOverride(Size finalSize) => ArrangeOverride(this, finalSize);

    public abstract Size MeasureOverride(IPanelAdapter adapter, Size availaibleSize);
    public abstract Size ArrangeOverride(IPanelAdapter adapter, Size finalSize);

    #region IPanelAdaptor Implementation

    void IPanelAdapter.Measure(int child, Size availableSize) => 
      InternalChildren[child].Measure(availableSize);

    Size IPanelAdapter.GetDesiredSize(int child) => InternalChildren[child].DesiredSize;

    void IPanelAdapter.Arrange(int child, Rect finalRect) => InternalChildren[child].Arrange(finalRect);

    int IPanelAdapter.ChildrenCount => InternalChildren.Count;

    object IPanelAdapter.GetChildProperty(int child, DependencyProperty property) =>
      InternalChildren[child].GetValue(property);

    #endregion
  }
}