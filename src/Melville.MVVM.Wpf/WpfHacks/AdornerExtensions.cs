using System;
using System.Windows.Documents;

namespace Melville.MVVM.Wpf.WpfHacks;

public static class AdornerExtensions
{
    public static Action ShowAdornerWithRemovalAction(this Adorner adorner)
    {
        var layer = AdornerLayer.GetAdornerLayer(adorner.AdornedElement);
        layer.Add(adorner);
        return () => layer.Remove(adorner);
    }

}