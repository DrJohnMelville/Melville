using System;
using System.Windows;
using System.Windows.Media;

namespace Melville.Linq.Statistics.Graphics.Internal
{

  public sealed class GraphArea : FrameworkElement
  {
    public GraphArea()
    {
      HorizontalAlignment = HorizontalAlignment.Stretch;
      VerticalAlignment = VerticalAlignment.Stretch;
    }

    public Graph Source
    {
      get { return (Graph)GetValue(SourceProperty); }
      set { SetValue(SourceProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register("Source", typeof(Graph), typeof(GraphArea), 
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange |
            FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsParentArrange |
            FrameworkPropertyMetadataOptions.AffectsParentMeasure |
            FrameworkPropertyMetadataOptions.AffectsRender));


    protected override void OnRender(DrawingContext drawingContext)
    {
      if (Source == null) return;
      drawingContext.DrawRectangle(Brushes.Transparent, Pens.Black, 
        new Rect(new Point(), DesiredSize));
      Source.SetActualExtent(desiredSize.Width, desiredSize.Height);
      Source.Render(new LowLevelGraphSurface(drawingContext));
    }

    Size desiredSize = new Size(100,100);
    protected override Size MeasureOverride(Size constraint)
    {
      double CheckDefinite(double d) => Double.IsInfinity(d) ? double.MaxValue : d;
      desiredSize = new Size(CheckDefinite(constraint.Width), CheckDefinite(constraint.Height));
      return desiredSize;
    }
  }
}