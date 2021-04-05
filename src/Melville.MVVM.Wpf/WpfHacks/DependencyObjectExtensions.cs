using  System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Melville.Linq;

namespace Melville.MVVM.Wpf.WpfHacks
{
  public static class DependencyObjectExtensions
  {
    /// <summary>
    /// Returns the passed object, followed by all its descendants
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public static IEnumerable<DependencyObject> Parents(this DependencyObject root) => 
      FunctionalMethods.Sequence(root, i => VisualTreeHelper.GetParent(i));

    public static IEnumerable<DependencyObject> Descendants(this DependencyObject item) => 
      (new[] { item }).SelectRecursive(Children);

    private static IEnumerable<DependencyObject> Children(DependencyObject source) => 
      Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(source)).Select(i => VisualTreeHelper.GetChild(source, i));
  }
}