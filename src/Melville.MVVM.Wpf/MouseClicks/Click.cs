using System.Windows;
using Melville.INPC;

namespace Melville.MVVM.Wpf.MouseClicks;

public static partial class Click
{
  [GenerateDP(Attached = true)]
  private static void OnLeftClickChanged(DependencyObject obj, string newValue) =>
    new MouseClickModelObject(1, (UIElement) obj, newValue ?? "")
      .Bind(UIElement.MouseLeftButtonDownEvent, UIElement.MouseLeftButtonUpEvent);

  [GenerateDP(Attached = true)]
  private static void OnPreviewLeftClickChanged(DependencyObject obj, string newValue) =>
    new MouseClickModelObject(1, (UIElement) obj, newValue ?? "")
      .Bind(UIElement.PreviewMouseLeftButtonDownEvent, UIElement.PreviewMouseLeftButtonUpEvent);

  [GenerateDP(Attached = true)]
  private static void OnRightClickChanged(DependencyObject obj, string newValue) =>
    new MouseClickModelObject(1, (UIElement) obj, newValue.ToString() ?? "")
      .Bind(UIElement.MouseRightButtonDownEvent, UIElement.MouseRightButtonUpEvent);

  [GenerateDP(Attached = true)]
  private static void OnDoubleClickChanged(DependencyObject obj, string newValue) =>
    new MouseClickModelObject(2, (UIElement) obj, newValue ?? "")
      .Bind(UIElement.MouseLeftButtonDownEvent, UIElement.MouseLeftButtonUpEvent);

  [GenerateDP(Attached = true)]
  private static void OnRightDoubleClickChanged(DependencyObject obj, string newValue) =>
    new MouseClickModelObject(2, (UIElement) obj, newValue ?? "")
      .Bind(UIElement.MouseRightButtonDownEvent, UIElement.MouseRightButtonUpEvent);
}