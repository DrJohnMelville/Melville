using System;
using System.Windows;
using System.Windows.Input;
using Melville.INPC;
using Melville.MVVM.Wpf.EventBindings.SearchTree;

namespace Melville.MVVM.Wpf.MouseClicks;

public sealed partial class MouseClickModelObject
{
    [FromConstructor] private readonly int targetClickCount;
    [FromConstructor] private readonly UIElement target;
    [FromConstructor] private readonly string method;
    private RoutedEvent? up;
    
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
            object?[] inputParams = new[] {args};
            new VisualTreeRunner(target as DependencyObject).RunTreeSearch(method, inputParams, out var _);
        }
    }
    private bool CheckDimension(double endPosition, double beginPosition) => Math.Abs(endPosition - beginPosition) < 5.0;
}