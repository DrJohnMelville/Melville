using  System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Melville.INPC;
using Melville.MVVM.Wpf.MouseDragging.Drop;
using Melville.MVVM.Wpf.WpfHacks;


namespace Melville.MVVM.Wpf.MouseDragging.ListRearrange
{
  public interface IAddSupplementalFormats
  {
    void AddFormats(IDataObject dataObject, object sourceObject);
  }

  public static partial class TreeArrange
  {
    #region DragTypeProperty

    [GenerateDP(typeof(Type))]
    private static void OnDragTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
      BindTreeArrange(d, e, false);

    [GenerateDP(typeof(Type))]
    private static void OnDragTypeBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
      BindTreeArrange(d, e, true);

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
            TreeDragDriver.BindToEvent(elt, dragInBackground, eltType);
            new TreeDropDriver(elt, eltType).AttachEvents(elt);
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
    
    [GenerateDP(typeof(FrameworkElement), "VisualToDrag", Attached = true, Nullable = true)]
    [GenerateDP(typeof(object), "DraggedItem", Attached = true)]
    [GenerateDP(typeof(IAddSupplementalFormats), "SupplementalFormats", Attached = true, Nullable = true)]
    [GenerateDP(typeof(IDropTarget), "SupplementalDropTarget", Attached = true, Nullable = true)]
    [GenerateDP]
    private static void OnSupplementalDropStringChanged(DependencyObject d, string? newValue)
    {
      if (d is FrameworkElement fe)
      {
        SetSupplementalDropTarget(fe, new DropTarget(fe, newValue ?? ""));
      }
    }

    public static object? FindDraggedItem(FrameworkElement fe) => GetDraggedItem(fe) ?? fe.DataContext;

    public static FrameworkElement? AdornmentTarget(object elt) => elt switch
    {
      DependencyObject dobj when GetVisualToDrag(dobj) is { } explicitTarget => explicitTarget,
      FrameworkElement fe => TryGetContainingCollectionViewItem(fe),
      _ => elt as FrameworkElement
    };

    private static FrameworkElement TryGetContainingCollectionViewItem(FrameworkElement fe) =>
      fe.Parents()
        .OfType<FrameworkElement>()
        .FirstOrDefault(i => i is ListViewItem || i is TreeViewItem || i is ListBoxItem) ?? fe;
    
  }
}