using  System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Melville.INPC;

namespace Melville.MVVM.Wpf.MouseDragging.Adorners;

public partial class RectangleAdorner : DropAdorner
{
  [FromConstructor]private readonly Rect bounds;

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