using  System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Melville.MVVM.Wpf.MouseDragging.Adorners
{
  public abstract class DropAdorner : Adorner
  {
    #region Adorner UI Element
    private UIElement? adornerUIElement;
    private UIElement AdornerUIElement
    {
      get
      {
        if (adornerUIElement == null)
        {
          adornerUIElement = CreateAdorningElement();
          AddVisualChild(adornerUIElement);
        }
        return adornerUIElement;
      }
    }
    protected abstract UIElement CreateAdorningElement();

    #endregion


    /// <summary>
    /// Initializes a new instance of the OutlineAdorner class.
    /// </summary>
    public DropAdorner(FrameworkElement adornedElement)
      : base(adornedElement)
    {
    }


    #region Layout
    protected override Size MeasureOverride(Size constraint)
    {
      AdornerUIElement.Measure(constraint);
      return AdornerUIElement.DesiredSize;
    }
    protected override Size ArrangeOverride(Size finalSize)
    {
      AdornerUIElement.Arrange(new Rect(finalSize));
      return finalSize;
    }
    protected override Visual GetVisualChild(int index) => AdornerUIElement;
    protected override int VisualChildrenCount => 1;

    #endregion
  }
}