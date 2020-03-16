#nullable disable warnings
using  System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Melville.MVVM.Wpf.MouseDragging.Adorners;
using Melville.MVVM.Wpf.MouseDragging.Drag;
using Melville.MVVM.Wpf.MouseDragging.Drop;
using Melville.MVVM.Wpf.WpfHacks;

namespace Melville.MVVM.Wpf.MouseDragging.ListRearrange
{
  public interface IAddSupplementalFormats
  {
    void AddFormats(IDataObject dataObject, object sourceObject);
  }
  public sealed class TreeArrange
  {
    #region DragTypeProperty

    public static Type GetDragType(DependencyObject obj) => (Type) obj.GetValue(DragTypeProperty);
    public static void SetDragType(DependencyObject obj, Type value) => obj.SetValue(DragTypeProperty, value);
    public static readonly DependencyProperty DragTypeProperty = DependencyProperty.RegisterAttached("DragType",
      typeof(Type), typeof(TreeArrange),
      new FrameworkPropertyMetadata(null, DragTypeChanged));

    private static void DragTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => BindTreeArrange(d,e,false);

    public static Type GetDragTypeBackground(DependencyObject ob) => (Type)ob.GetValue(DragTypeBackgroundProperty);
    public static void SetDragTypeBackground(DependencyObject ob, Type value) => ob.SetValue(DragTypeBackgroundProperty, value);
    public static readonly DependencyProperty DragTypeBackgroundProperty =
      DependencyProperty.RegisterAttached("DragTypeBackground",
        typeof(Type), typeof(TreeArrange), new FrameworkPropertyMetadata(null, DragTypeBackgroundChanged));

    private static void DragTypeBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => BindTreeArrange(d, e, true);

    private static void BindTreeArrange(DependencyObject d, DependencyPropertyChangedEventArgs e, bool dragInBackground)
    {
      if (e.NewValue is Type eltType)
      {
        switch (d)
        {
          case ItemsControl ic:
            ic.ItemContainerStyle = ItemContainerStyle(ic.ItemContainerStyle, eltType, dragInBackground);
            break;
          case FrameworkElement elt:
            new TreeDropDriver(elt, eltType).AttachEvents(elt, dragInBackground);
            break;          
        }
      }
    }

    public static Style ItemContainerStyle(Style baseStyle, Type eltType, bool bindBackground)
    {
      var newStyle = baseStyle == null ? new Style() : new Style(baseStyle.TargetType, baseStyle);

      newStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.Transparent));
      newStyle.Setters.Add(new Setter(bindBackground ? DragTypeBackgroundProperty : DragTypeProperty, eltType));

      return newStyle;
    }


    #endregion

    #region VisualToDrag


    public static FrameworkElement GetVisualToDrag(DependencyObject obj)
    {
      return (FrameworkElement)obj.GetValue(VisualToDragProperty);
    }

    public static void SetVisualToDrag(DependencyObject obj, FrameworkElement value)
    {
      obj.SetValue(VisualToDragProperty, value);
    }

    // Using a DependencyProperty as the backing store for VisualToDrag.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty VisualToDragProperty =
        DependencyProperty.RegisterAttached("VisualToDrag", typeof(FrameworkElement), typeof(TreeArrange), new PropertyMetadata(null));




    #endregion

    #region ItemToDrag



    public static object GetDraggedItem(DependencyObject obj)
    {
      return (object)obj.GetValue(DraggedItemProperty);
    }

    public static void SetDraggedItem(DependencyObject obj, object value)
    {
      obj.SetValue(DraggedItemProperty, value);
    }

    // Using a DependencyProperty as the backing store for DraggedItem.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DraggedItemProperty =
        DependencyProperty.RegisterAttached("DraggedItem", typeof(object), typeof(TreeArrange), new PropertyMetadata(null));



    #endregion

    #region SupplementalFormats

    public static IAddSupplementalFormats GetSupplementalFormats(DependencyObject obj) =>
      (IAddSupplementalFormats) obj.GetValue(SupplementalFormatsProperty);

    public static void SetSupplementalFormats(DependencyObject obj, IAddSupplementalFormats format) =>
      obj.SetValue(SupplementalFormatsProperty, format);
    public static readonly DependencyProperty SupplementalFormatsProperty = DependencyProperty.RegisterAttached(
      "SupplementalFormats", typeof(IAddSupplementalFormats), typeof(TreeArrange), new PropertyMetadata(null));
    #endregion

    #region Backup Dropper
    public static IDropTarget GetSupplementalDropTarget(DependencyObject obj) =>
      (IDropTarget) obj.GetValue(SupplementalDropTargetProperty);
    public static void SetSupplementalDropTarget(DependencyObject obj, IDropTarget value) => 
      obj.SetValue(SupplementalDropTargetProperty, value);
    public static readonly DependencyProperty SupplementalDropTargetProperty =
      DependencyProperty.RegisterAttached("SupplementalDropTarget", typeof(IDropTarget), typeof(TreeArrange), 
        new PropertyMetadata(null));



    public static string GetSupplementalDropString(DependencyObject obj) => 
      (string)obj.GetValue(SupplementalDropStringProperty);
    public static void SetSupplementalDropString(DependencyObject obj, string value) => 
      obj.SetValue(SupplementalDropStringProperty, value);
    public static readonly DependencyProperty SupplementalDropStringProperty =
        DependencyProperty.RegisterAttached("SupplementalDropString", typeof(string), typeof(TreeArrange), 
          new FrameworkPropertyMetadata(null, SetSupplementalDrop));

    private static void SetSupplementalDrop(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (d is FrameworkElement fe)
      {
        SetSupplementalDropTarget(fe, new DropTarget(fe, e.NewValue.ToString()));
      }
    }

    #endregion

    private class TreeDropDriver
     {
       #region Construction and Binding

       private readonly FrameworkElement rootElt;
       private readonly Type dropType;
       private string DragTypeName() => dropType.FullName;

       public TreeDropDriver(FrameworkElement  rootElt, Type dropType)
       {
         this.rootElt = rootElt;
         this.dropType = dropType;
       }

       public void AttachEvents(FrameworkElement elt, bool dragInBackground)
       {
         elt.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, (MouseButtonEventHandler)InitiateDrag, dragInBackground);
         elt.AllowDrop = true;
         elt.DragOver += DragOver;
         elt.DragLeave += DragLeave;
         elt.Drop += Drop;
       }

       #endregion

       #region Drag

       private void InitiateDrag(object sender, MouseButtonEventArgs e)
       {
         if (sender is FrameworkElement fe && dropType.IsInstanceOfType(FindDraggedItem(fe)))
         {
           CreateDragger(e, fe)
             .RequireInitialDelta()
             .DragTarget(0.5)
             .Drag(GetDataObject(fe), DragDropEffects.All,
               RemoveMovedItemFromList);

           void RemoveMovedItemFromList(DragDropEffects operation)
           {
           //    FindList(fe, FindDraggedItem(fe))?.Remove(fe.DataContext);
           }
         }
       }

       private static IMouseDataSource CreateDragger(MouseButtonEventArgs e, FrameworkElement fe) =>
         GetVisualToDrag(fe) is FrameworkElement dragTarget
           ? new MouseDragger(dragTarget, e).LeafTarget()
           : new MouseDragger(fe, e).TypedTarget(typeof(ListViewItem), typeof(TreeViewItem), typeof(ListBoxItem));

       private static object FindDraggedItem(FrameworkElement fe)
       {
         return GetDraggedItem(fe) ?? fe.DataContext;
       }

       private DataObject GetDataObject(FrameworkElement fe)
       {
         var data = FindDraggedItem(fe);
         var ret = new DataObject(DragTypeName(), data);
         TreeArrange.GetSupplementalFormats(fe)?.AddFormats(ret, data);
         return ret;
       }

       #endregion

       #region Adorn on drag

       private void DragLeave(object sender, DragEventArgs e)
       {
         if (SupplementalDropTarget() is IDropTarget target)
         {
           target.DragLeave(sender, e);
         }
         AdornmentTarget(sender)?.ClearAdorners();
       }

       private void DragOver(object sender, DragEventArgs e)
       {
         if (AdornmentTarget(sender) is FrameworkElement fe)
         {
           fe.ClearAdorners();
           if (e.Data.GetData(DragTypeName()) == null)
           {
             if (SupplementalDropTarget() is IDropTarget target)
             {
               target.DragOver(sender, e);
               return;
             }
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

       private IDropTarget SupplementalDropTarget()
       {
         return GetSupplementalDropTarget(rootElt);
       }

       private FrameworkElement AdornmentTarget(object elt)
       {
         return 
           GetVisualToDrag(elt as DependencyObject) ??
            (elt is FrameworkElement fe
           ? fe.Parents().OfType<FrameworkElement>().FirstOrDefault(i => i is ListViewItem || i is TreeViewItem || i is ListBoxItem) ?? fe
           : null);
       }


       private static DropAdornerKind DropTypeByPosition(DragEventArgs e, FrameworkElement fe) =>
         e.GetPosition(fe).Y / fe.ActualHeight > 0.5 ? DropAdornerKind.Bottom : DropAdornerKind.Top;

       #endregion

       #region Drop

       private void Drop(object sender, DragEventArgs e)
       {
         if (AdornmentTarget(sender) is FrameworkElement fe)
         {
           fe.ClearAdorners();
           var draggedItem = e.Data.GetData(DragTypeName());
           if (draggedItem == null)
           {
             if (SupplementalDropTarget() is IDropTarget innerTarget)
             {
               innerTarget.HandleDrop(sender, e);
             }
            return;
           }
           var target = FindDraggedItem(fe);
           if (target == null || target == draggedItem)  return;
           var items = FindList(fe, target);

           if (items == null || items is Array) return;
           if (items.Contains(draggedItem))
           {
             items.Remove(draggedItem);
             e.Effects = DragDropEffects.Copy;
           }
           else
           {
             e.Effects = DragDropEffects.Copy;
           }

           e.Handled = true;

           var index = items.IndexOf(target) + (DropTypeByPosition(e, fe) == DropAdornerKind.Bottom ? 1 : 0);
           items.Insert(index, draggedItem);
         }
       }

       private IList FindList(FrameworkElement targetElement, object targetData) =>
         targetElement.Parents().OfType<ItemsControl>()
           .Select(i => i.ItemsSource)
           .OfType<IList>()
           .FirstOrDefault(i => i.Contains(targetData));

       #endregion
     }
  }
}