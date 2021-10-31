using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Melville.Linq;
using Melville.MVVM.Wpf.WpfHacks;

namespace Melville.MVVM.Wpf.MouseDragging.ListRearrange;

public static class ListFinder
{
    public static bool IsAMutableListOf(object? item, Type itemType) => 
        item != null && IsAMutableListOf(item.GetType(), itemType);

    public static bool IsAMutableListOf(Type baseType, Type itemType)
    {
        if (baseType.IsArray)
            return false; // array implements IList but throws on mutation, so not good enough for us
        var types = baseType.GetInterfaces();
        var listTypes = types.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>))
            .ToList();
        if (listTypes.Any()) 
            return listTypes.Any(i => i.GetGenericArguments()[0].IsAssignableFrom(itemType));
        return types.Any(i => i == typeof(IList));
    }
        
        
    public static IList? FindChildListToHoldData(FrameworkElement targetElement, object targetData) =>
        FindListToHoldObject(targetElement.Descendants(), targetData)
            .FirstOrDefault();
     
    public static IList? FindParentListContainingData(FrameworkElement targetElement, object targetData) =>
        FindListToHoldObject(targetElement.Parents(), targetData)
            .FirstOrDefault(i => i.Contains(targetData));

    private static IEnumerable<IList> FindListToHoldObject(IEnumerable<DependencyObject> sourceList,
        object targetData) =>
        sourceList.OfType<ItemsControl>()
            .Select(i => i.ItemsSource)
            .Where(i=>IsAMutableListOf(i, targetData.GetType()))
            .OfType<IList>();
}