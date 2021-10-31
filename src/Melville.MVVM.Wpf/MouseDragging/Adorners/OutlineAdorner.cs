using  System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.Adorners;

public sealed class OutlineAdorner : RectangleAdorner
{
  public OutlineAdorner(FrameworkElement adornedElement) :
    base(adornedElement, new Rect(new Size(adornedElement.ActualWidth, adornedElement.ActualHeight)))
  {
  }

}