using  System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Melville.INPC;
using Melville.Linq;

namespace Melville.MVVM.Wpf.EventBindings
{
  public interface IAdditionlTargets
  {
    IEnumerable<object> Targets();
  }
  [GenerateDP(typeof(object), "Target", Attached = true, Nullable = true)]
  public static partial class TargetSelector
  {
    public static IEnumerable<object> ResolveTarget(DependencyObject sender) => 
      sender.AllSources(GetTarget(sender));

    public static IEnumerable<object> AllSources(this DependencyObject sender,
      params object?[] firstItems) =>
      firstItems
        .Concat(InnerSources(sender).SelectMany(SearchAdditionalTargets))
        .Append(Application.Current)
        .OfType<object>()
        .Distinct();

    private static IEnumerable<object> InnerSources(DependencyObject sender)
    {
      DependencyObject? current = sender;
      while (current != null)
      {
        yield return current.GetValue(FrameworkElement.DataContextProperty);
        yield return current;
        var prior = current;
        current = VisualParent(current) ??
                  LogicalTreeHelper.GetParent(prior) ??
                  (current as Popup)?.PlacementTarget;
      }
    }

    private static DependencyObject? VisualParent(DependencyObject current) =>
      current switch
      {
        Visual vis => VisualTreeHelper.GetParent(current),
        Visual3D vis => VisualTreeHelper.GetParent(current),
        _ => null
      };


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