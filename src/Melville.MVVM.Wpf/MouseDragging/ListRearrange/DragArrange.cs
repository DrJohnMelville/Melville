#nullable disable warnings
using  System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Melville.MVVM.Wpf.MouseDragging.Adorners;
using Melville.MVVM.Wpf.MouseDragging.Drag;
using Melville.MVVM.Wpf.MouseDragging.Drop;

namespace Melville.MVVM.Wpf.MouseDragging.ListRearrange
{
    [Obsolete("Movew to TreeArranger")]
  public sealed class DragArrange
  {


    public static IList GetList(DependencyObject obj) => (IList)obj.GetValue(ListProperty);
    public static void SetList(DependencyObject obj, IList value) => obj.SetValue(ListProperty, value);

    // Using a DependencyProperty as the backing store for List.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ListProperty =
        DependencyProperty.RegisterAttached("List", typeof(IList), typeof(DragArrange), 
          new FrameworkPropertyMetadata(null, AddList));

    private static void AddList(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (d is ItemsControl ic)
      {
        var driver = new ListDragDropMonitor((IList)(e.NewValue));
        ic.ItemContainerStyle = driver.ItemContainerStyle(ic.ItemContainerStyle);
      }
    }

    private class ListDragDropMonitor
    {
      private readonly IList items;
      private readonly Type instanceType;

      public ListDragDropMonitor(IList item)
      {
        this.items = item;
        this.instanceType = GetElementType(item);
      }

      public Style ItemContainerStyle(Style? baseStyle)
      {
        var newStyle = baseStyle == null ? new Style() : new Style(baseStyle.TargetType, baseStyle);

        newStyle.Setters.Add(new EventSetter(UIElement.MouseLeftButtonDownEvent, (MouseButtonEventHandler)DragInitiate));
        newStyle.Setters.Add(new EventSetter(UIElement.DragOverEvent, (DragEventHandler)DragOver));
        newStyle.Setters.Add(new EventSetter(UIElement.DragLeaveEvent, (DragEventHandler)DragLeave));
        newStyle.Setters.Add(new EventSetter(UIElement.DropEvent, (DragEventHandler)Drop));
        newStyle.Setters.Add(new Setter(UIElement.AllowDropProperty, true));
        newStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.Aqua));

        return newStyle;
      }

      private void Drop(object sender, DragEventArgs e)
      {
        if (sender is FrameworkElement fe)
        {
          fe.ClearAdorners();
          var draggedItem = e.Data.GetData(instanceType);
          if (draggedItem == null) return;
          var target = fe.DataContext;
          if (target == null || !items.Contains(target)) return;
          if (items.Contains(draggedItem))
          {
            items.Remove(draggedItem);
          }
          e.Effects = DragDropEffects.Copy;

          var index = items.IndexOf(target) + (DropTypeByPosition(e, fe) == DropAdornerKind.Bottom ? 1 : 0);
          items.Insert(index, draggedItem);
        }
      }

      private void DragLeave(object sender, DragEventArgs e)
      {
        if (sender is FrameworkElement fe)
        {
          fe.ClearAdorners();
        }
      }

      private void DragOver(object sender, DragEventArgs e)
      {
        if (sender is FrameworkElement fe)
        {
          fe.ClearAdorners();
          if (e.Data.GetData(instanceType) == null)
          {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
          };
          var dropAdornerKind = DropTypeByPosition(e, fe);
          fe.Adorn(dropAdornerKind);
          e.Effects = DragDropEffects.Copy;
          e.Handled = true;
        }
      }

      private static DropAdornerKind DropTypeByPosition(DragEventArgs e, FrameworkElement fe) => 
        e.GetPosition(fe).Y / fe.ActualHeight > 0.5 ? DropAdornerKind.Bottom : DropAdornerKind.Top;

      private void DragInitiate(object s, MouseButtonEventArgs e)
      {
        new MouseDragger(s as DependencyObject, e).LeafTarget()
          .DragTarget(0.5)
          .Drag(new DataObject(((FrameworkElement) s).DataContext), DragDropEffects.Copy);
      }

      private static Type GetElementType(object targetCollection)
      {
        var collType = targetCollection.GetType();
        if (collType.IsGenericType)
        {
          foreach (var interfaceType in collType.GetInterfaces())
          {
            if (interfaceType.GetGenericTypeDefinition() != typeof(ICollection<>)) continue;
            return interfaceType.GenericTypeArguments[0];
          }
        }
        return typeof(object);
      }

    }
  }
}