using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Melville.MVVM.Wpf.WpfHacks;

namespace Melville.MVVM.Wpf.VisualTreeLocations;

public interface IVisualTreeLocation<TChild, TTarget>
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    TTarget Target { get; }
    [EditorBrowsable(EditorBrowsableState.Never)]
    TChild CreateNewChild(TTarget? target);
}

public static class VisualTreeLocationOperations
{
    public static TChild AttachToTop<TChild, TTarget>(this IVisualTreeLocation<TChild, TTarget> holder) 
        where TTarget : DependencyObject =>
        holder.CreateNewChild(ElligibleParents(holder.Target).Last());

    public static TChild AttachToName<TChild, TTarget>(this IVisualTreeLocation<TChild, TTarget> holder, string name) 
        where TTarget : FrameworkElement =>
        holder.CreateNewChild(ElligibleParents(holder.Target)
            .FirstOrDefault(i => i.Name?.Equals(name, StringComparison.Ordinal)??false));

    public static TChild AttachToType<TChild, TTarget>(this IVisualTreeLocation<TChild, TTarget> holder, Type type) 
        where TTarget : DependencyObject =>
        holder.CreateNewChild(ElligibleParents(holder.Target)
            .FirstOrDefault(type.IsInstanceOfType));

    public static TChild AttachToDataContextHolder<TChild, TTarget>(
        this IVisualTreeLocation<TChild, TTarget> holder, Type type) where TTarget : FrameworkElement =>
        holder.CreateNewChild(ElligibleParents(holder.Target)
            .FirstOrDefault(i=>type.IsInstanceOfType(i.DataContext)));
    public static TChild AttachToDataContextHolder<TChild, TTarget>(
        this IVisualTreeLocation<TChild, TTarget> holder, params Type[] type) where TTarget : FrameworkElement =>
        holder.CreateNewChild(ElligibleParents(holder.Target)
            .FirstOrDefault(i=>type.Any(j=>j.IsInstanceOfType(i.DataContext))));

    private static IEnumerable<TTarget> ElligibleParents<TTarget>(TTarget? target)
        where TTarget : DependencyObject =>
        target?.Parents().OfType<TTarget>() ?? Array.Empty<TTarget>();

}