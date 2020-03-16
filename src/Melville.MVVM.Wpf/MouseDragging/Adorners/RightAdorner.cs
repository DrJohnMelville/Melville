using  System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Melville.MVVM.Wpf.MouseDragging.Adorners
{
  public sealed class RightAdorner : DropAdorner
  {
    public RightAdorner(FrameworkElement adornedElement) : base(adornedElement)
    {
    }
    protected override UIElement CreateAdorningElement()
    {
      var elt = (FrameworkElement)AdornedElement;
      return new Line
      {
        Stroke = Brushes.Gold,
        StrokeThickness = 2,
        X1 = elt.ActualWidth - 2,
        X2 = elt.ActualWidth - 2,
        Y1 = 0,
        Y2 = elt.ActualHeight
      };
    }
  }
}