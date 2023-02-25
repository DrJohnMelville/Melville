using System;
using  System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Melville.INPC;
using Melville.Linq;

namespace Melville.MVVM.Wpf.EventBindings;

public interface IAdditionlTargets
{
  IEnumerable<object> Targets();
}
[GenerateDP(typeof(object), "Target", Attached = true, Nullable = true,
    XmlDocumentation = """
    Indicates an additional target that should be searched for treerunner requests
    to execute a method on this node.
    """)]
public static partial class TargetSelector
{
  public static IList<object> ResolveTarget(DependencyObject sender, 
    TargetListCompositeExpander expander) =>
    expander.Expand(AugmentedTargetList(sender));

  private static IEnumerable<object?> AugmentedTargetList(DependencyObject sender) =>
    ItemAndAllParents(sender)
      .Append(Application.Current)
      .Prepend(GetTarget(sender));

  private static IEnumerable<object?> ItemAndAllParents(DependencyObject sender) => 
    FunctionalMethods.Sequence<DependencyObject?>(sender, AggressiveSearchForParent);

  private static DependencyObject? AggressiveSearchForParent(DependencyObject? current) =>
    current == null?null:
      VisualParent(current) ??
      LogicalTreeHelper.GetParent(current) ??
      (current as Popup)?.PlacementTarget;

  private static DependencyObject? VisualParent(DependencyObject current) =>
    current switch
    {
      Visual or Visual3D => VisualTreeHelper.GetParent(current),
      _ => null
    };
    
}