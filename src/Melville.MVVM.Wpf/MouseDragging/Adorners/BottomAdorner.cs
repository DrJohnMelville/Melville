using  System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Melville.MVVM.Wpf.MouseDragging.Adorners
{
  public sealed class BottomAdorner : DropAdorner
  {
    public BottomAdorner(FrameworkElement adornedElement) : base(adornedElement)
    {
    }

    protected override UIElement CreateAdorningElement()
    {
      var elt = (FrameworkElement)AdornedElement;
      return new Line
      {
        Stroke = Brushes.Gold,
        StrokeThickness = 2,
        X1 = 0,
        X2 = elt.ActualWidth,
        Y1 = elt.ActualHeight - 2,
        Y2 = elt.ActualHeight - 2
      };
    }
  }
}