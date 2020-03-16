using  System;
using System.Windows;
using System.Windows.Input;

namespace Melville.MVVM.Wpf.MouseClicks
{
  public static class Click
  { 
    #region Left Click handler
    public static DependencyProperty LeftClickProperty = DependencyProperty.RegisterAttached("LeftClick",
      typeof(string), typeof(Click), new FrameworkPropertyMetadata("", LeftClickChanged));
    public static string GetLeftClick(DependencyObject dependencyObject)
    {
      return dependencyObject.GetValue(LeftClickProperty).ToString()??"";
    }
    public static void SetLeftClick(DependencyObject obj, string value)
    {
      obj.SetValue(LeftClickProperty, value);
    }
    private static void LeftClickChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) => 
      new MouseClickModelObject(1, (UIElement)obj, e.NewValue.ToString()??"")
        .Bind(UIElement.MouseLeftButtonDownEvent, UIElement.MouseLeftButtonUpEvent);

    #endregion
    #region Left Click handler
    public static DependencyProperty PreviewLeftClickProperty = DependencyProperty.RegisterAttached("PreviewLeftClick",
      typeof(string), typeof(Click), new FrameworkPropertyMetadata("", PreviewLeftClickChanged));
    public static string GetPreviewLeftClick(DependencyObject dependencyObject)
    {
      return dependencyObject.GetValue(PreviewLeftClickProperty).ToString()??"";
    }
    public static void SetPreviewLeftClick(DependencyObject obj, string value)
    {
      obj.SetValue(PreviewLeftClickProperty, value);
    }
    private static void PreviewLeftClickChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
      new MouseClickModelObject(1, (UIElement)obj, e.NewValue.ToString()??"")
        .Bind(UIElement.PreviewMouseLeftButtonDownEvent, UIElement.PreviewMouseLeftButtonUpEvent);

    #endregion
    #region Right Click handler
    public static DependencyProperty RightClickProperty = DependencyProperty.RegisterAttached("RightClick",
      typeof(string), typeof(Click), new FrameworkPropertyMetadata("", RightClickChanged));
    public static string GetRightClick(DependencyObject dependencyObject)
    {
      return dependencyObject.GetValue(RightClickProperty).ToString()??"";
    }
    public static void SetRightClick(DependencyObject obj, string value)
    {
      obj.SetValue(RightClickProperty, value);
    }
    private static void RightClickChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
      new MouseClickModelObject(1, (UIElement)obj, e.NewValue.ToString()??"")
        .Bind(UIElement.MouseRightButtonDownEvent, UIElement.MouseRightButtonUpEvent);

    #endregion

    #region Double Click Handler
    public static DependencyProperty DoubleClickProperty = DependencyProperty.RegisterAttached("DoubleClick",
      typeof(string), typeof(Click), new FrameworkPropertyMetadata("", DoubleClickChanged));
    public static string GetDoubleClick(DependencyObject dependencyObject)
    {
      return dependencyObject.GetValue(DoubleClickProperty).ToString()??"";
    }
    public static void SetDoubleClick(DependencyObject obj, string value)
    {
      obj.SetValue(DoubleClickProperty, value);
    }
    private static void DoubleClickChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
      new MouseClickModelObject(2, (UIElement)obj, e.NewValue.ToString()??"")
        .Bind(UIElement.MouseLeftButtonDownEvent, UIElement.MouseLeftButtonUpEvent);

    public static DependencyProperty RightDoubleClickProperty = DependencyProperty.RegisterAttached("RightDoubleClick",
      typeof(string), typeof(Click), new FrameworkPropertyMetadata("", RightDoubleClickChanged));
    public static string GetRightDoubleClick(DependencyObject dependencyObject)
    {
      return dependencyObject.GetValue(RightDoubleClickProperty).ToString()??"";
    }
    public static void SetRightDoubleClick(DependencyObject obj, string value)
    {
      obj.SetValue(RightDoubleClickProperty, value);
    }
    private static void RightDoubleClickChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
      new MouseClickModelObject(2, (UIElement)obj, e.NewValue.ToString()??"")
        .Bind(UIElement.MouseRightButtonDownEvent, UIElement.MouseRightButtonUpEvent);



    #endregion


    private sealed class MouseClickModelObject
    {
      private readonly string method;
      private readonly int targetClickCount;
      private readonly UIElement target;
      private RoutedEvent? up;

      public MouseClickModelObject(int targetClickCount, UIElement target, string method)
      {
        this.method = method;
        this.target = target;
        this.targetClickCount = targetClickCount;
      }

      public void Bind(RoutedEvent down, RoutedEvent up)
      { 
          target.AddHandler(down, (MouseButtonEventHandler) ClickDown);
          this.up = up;
      }

      /* There are two competing facets here that led to an obscure code trick.  
          * 
          * I need to NOT handle this event.  If I handle it then a double click handler
          * a list item prevents selection in the list box.  On the other hand the
          * photos have a double click handler, inside of which there is a downloading
          * icon with a click handler to cancel the download.  If I do not handle the event
          * the outer double click handler, being the last one to capture the mouse eats
          * the click away from the single click handler.
          *
          * The first point is that I will not fire twice for the same time stamp, so only the innermost click gets
          * initiated.  Then I register the up handler in the down handler, so only the first click gets to complete.
         */

      private Point firstPosition;
      private static int lastTimeStamp;

      private void ClickDown(object sender, MouseButtonEventArgs args)
      {
        if (args.Timestamp == lastTimeStamp) return;
        lastTimeStamp = args.Timestamp;

        firstPosition = args.GetPosition(target);

        if (ThisClickCompletesGesture(args))
        {
          target.AddHandler(up, (MouseButtonEventHandler) ClickUp);
        }
      }

      private bool ThisClickCompletesGesture(MouseButtonEventArgs args) => 
        (args.ClickCount == targetClickCount) || targetClickCount < 1;

      private void ClickUp(object sender, MouseButtonEventArgs args)
      {
        target.RemoveHandler(up, (MouseButtonEventHandler) ClickUp);

        var finalPosition = args.GetPosition(target);

        if (CheckDimension(finalPosition.X, firstPosition.X) &&
            CheckDimension(finalPosition.Y, firstPosition.Y))
        {
          SearchTree.RunOnVisualTreeSearch.Run(target as DependencyObject, method, new[] {args}, out var _);
        }
      }
      private bool CheckDimension(double endPosition, double beginPosition) => Math.Abs(endPosition - beginPosition) < 5.0;
    }
  }
}