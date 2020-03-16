using  System;
using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.Adorners
{
  public enum DropAdornerKind
  {
    Rectangle = 0,
    Top = 1,
    Left = 2,
    Bottom = 3,
    Right = 4
  }

  public static class DropAdornerFactory
  {
    public static DropAdorner Create(DropAdornerKind kind, FrameworkElement adornedElement)
    {
      switch (kind)
      {
        case DropAdornerKind.Rectangle:
          return new OutlineAdorner(adornedElement);
        case DropAdornerKind.Top:
          return new TopAdorner(adornedElement);
        case DropAdornerKind.Left:
          return new LeftAdorner(adornedElement);
        case DropAdornerKind.Bottom:
          return new BottomAdorner(adornedElement);
        case DropAdornerKind.Right:
          return new RightAdorner(adornedElement);
        default:
          throw new ArgumentOutOfRangeException("Unkown type of dropAdorner");
      }
    }
  }
}