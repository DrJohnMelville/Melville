using  System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Melville.MVVM.Wpf.MouseDragging.Adorners
{
  public  class RectangleAdorner : DropAdorner
  {
    private readonly Rect bounds;

    public RectangleAdorner(FrameworkElement adornedElement, Rect bounds) : base(adornedElement)
    {
      this.bounds = bounds;
    }
    protected override UIElement CreateAdorningElement()
    {
      var ret = new Rectangle
      {
        Stroke = Brushes.Gold,
        StrokeThickness = 2,
        Width = bounds.Width,
        Height = bounds.Height,
        RenderTransform = new TranslateTransform(bounds.X, bounds.Y)
      };
      return ret;
    }
  }
}