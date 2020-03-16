using  System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Melville.MVVM.Functional;

namespace Melville.MVVM.Wpf.EventBindings
{
  public interface IAdditionlTargets
  {
    IEnumerable<object> Targets();
  }
  public static class TargetSelector
  {
    public static DependencyProperty TargetProperty = DependencyProperty.RegisterAttached("Target", typeof(object), typeof(TargetSelector),
      new FrameworkPropertyMetadata(null));
    public static object GetTargetProperty(DependencyObject obj) => obj.GetValue(TargetProperty);
    public static void SetTargetProperty(DependencyObject obj, object value) => obj.SetValue(TargetProperty, value);

    public static IEnumerable<object> ResolveTarget(DependencyObject sender) => 
      sender.AllSources(GetTargetProperty(sender));

    public static IEnumerable<object> AllSources(this DependencyObject sender,
      params object[] firstItems)
    {
      return InnerSources()
        .SelectMany(SearchAdditionalTargets)
        .Prepend(firstItems).Where(i => i != null).Distinct();

      IEnumerable<object> InnerSources()
      {
        DependencyObject? current = sender;
        while (current != null)
        {
          yield return current.GetValue(FrameworkElement.DataContextProperty);
          yield return current;
          var prior = current;
          current = VisualTreeHelper.GetParent(current) ??
                    LogicalTreeHelper.GetParent(prior) ??
                    (current as Popup)?.PlacementTarget;
        }
      }
    }

    private static IEnumerable<object> SearchAdditionalTargets(object arg)
    {
      if (arg is IAdditionlTargets iat)
      {
        return iat.Targets().Concat(arg);
      }

      return EnumerableExtensions.FromObject(arg);
    }
  }
}