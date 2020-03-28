using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Melville.Linq.Statistics.DescriptiveStats;
using Melville.Linq.Statistics.Functional;

namespace Melville.Linq.Statistics.Graphics.Internal.Axes
{
  public sealed class AxisPanel: Panel
  {
    public static readonly DependencyProperty OffsetProperty = DependencyProperty.RegisterAttached("Offset",
      typeof(double), typeof(AxisPanel), new FrameworkPropertyMetadata(0.0,
        FrameworkPropertyMetadataOptions.AffectsMeasure |
        FrameworkPropertyMetadataOptions.AffectsParentMeasure));
    public static double GetOffset(DependencyObject obj) => (double) obj.GetValue(OffsetProperty);
    public static void SetOffset(DependencyObject obj, double value) => obj.SetValue(OffsetProperty, value);

    public static readonly DependencyProperty LevelProperty = DependencyProperty.RegisterAttached("Level",
      typeof(int), typeof(AxisPanel), new FrameworkPropertyMetadata(0,
        FrameworkPropertyMetadataOptions.AffectsMeasure |
        FrameworkPropertyMetadataOptions.AffectsParentMeasure));
    public static int GetLevel(DependencyObject obj) => (int) obj.GetValue(LevelProperty);
    public static void SetLevel(DependencyObject obj, int value) => obj.SetValue(LevelProperty, value);

    public Dock Location
    {
      get { return (Dock)GetValue(LocationProperty); }
      set { SetValue(LocationProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Location.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LocationProperty =
        DependencyProperty.Register("Location", typeof(Dock), typeof(AxisPanel), new PropertyMetadata(Dock.Bottom));

    protected override Size MeasureOverride(Size availableSize)
    {
      var computer = CreateLocationComputer(FixSize(availableSize));
      foreach (UIElement child in computer.SortChildren(InternalChildren))
      {
        child.Measure(availableSize);
        computer.PlaceControl(child);
      }
      return FixSize(computer.CurrentExtent.Size);
    }

    private LocationComputer CreateLocationComputer(Size size) => 
      LocationComputer.Factory(Location, size);

    private Size FixSize(Size finalRectSize) => 
      new Size(FixDouble(finalRectSize.Width), FixDouble(finalRectSize.Height));

    private double FixDouble(double num) => num.IsValidAndFinite()? num:2;

    protected override Size ArrangeOverride(Size finalSize)
    {
      var computer = CreateLocationComputer(finalSize);
      foreach (UIElement child in computer.SortChildren(InternalChildren))
      {
        var desiredRect = computer.PlaceControl(child);
        child.Arrange(desiredRect);
      }
      return FixSize(finalSize);
    }

    private abstract class LocationComputer
    {
      protected Size ControlSize { get; }
      protected LocationComputer(Size controlSize)
      {
        ControlSize = controlSize;
      }
      public Rect CurrentExtent { get; private set; } = new Rect();

      private void AddRectToExtent(Rect newRect) => 
        CurrentExtent = Rect.Union(CurrentExtent, newRect);

      public Rect PlaceControl(UIElement control)
      {
        var desiredSize = control.DesiredSize;
        var offset = GetOffset(control);
        var ret = InnerPlaceControl(desiredSize, offset);
        AddRectToExtent(ret);
        return ret;
      }

      protected abstract Rect InnerPlaceControl(Size size, double offset);
      protected abstract void NextRow();

      public IEnumerable<UIElement> SortChildren(UIElementCollection children)
      {
        var levels = children.OfType<UIElement>().GroupBy(GetLevel).
          OrderByDescending(SortingValue).AsList();

        foreach (var level in levels)
        {
          foreach (var child in level)
          {
            yield return child;
          }
          NextRow();
        }
      }

      protected virtual int SortingValue(IGrouping<int, UIElement> i)
      {
        return i.Key;
      }

      public static LocationComputer Factory(Dock which, Size size)
      {
        switch (which)
        {
          case Dock.Left: return new LeftComputer(size);
          case Dock.Right: return new VerticalComputer(size);
          default: return new HorizontalComputer(size);
        }
      }

      private sealed class HorizontalComputer : LocationComputer
      {
        private double yLocation = 0.0;
        protected override void NextRow() => yLocation = CurrentExtent.Height + 1;
        protected override Rect InnerPlaceControl(Size size, double offset)
        {
          return new Rect(new Point((ControlSize.Width*offset) - (size.Width / 2), yLocation), size);
        }

        public HorizontalComputer(Size controlSize) : base(controlSize)
        {
        }
      }
      private class VerticalComputer : LocationComputer
      {
        private double xLocation = 0.0;
        protected override void NextRow() => xLocation = CurrentExtent.Width + 1;
        protected override Rect InnerPlaceControl(Size size, double offset) => 
          new Rect(new Point(xLocation, (ControlSize.Height *offset) - (size.Height/ 2)), size);

        public VerticalComputer(Size controlSize) : base(controlSize)
        {
        }
      }

      private sealed class LeftComputer : VerticalComputer
      {
        #region Overrides of LocationComputer

        protected override int SortingValue(IGrouping<int, UIElement> i)
        {
          return -1 * base.SortingValue(i);
        }

        #endregion

        public LeftComputer(Size controlSize) : base(controlSize)
        {
        }
      }
    }
    #region Overrides of Panel

    protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
    {
      base.OnVisualChildrenChanged(visualAdded, visualRemoved);
      InvalidateMeasure();
      InvalidateArrange();
      InvalidateVisual();
      UpdateLayout();
    }

    #endregion
  }
}