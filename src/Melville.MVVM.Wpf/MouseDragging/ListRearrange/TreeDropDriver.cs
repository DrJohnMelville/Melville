using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Melville.MVVM.Wpf.MouseDragging.Adorners;
using Melville.MVVM.Wpf.MouseDragging.Drag;
using Melville.MVVM.Wpf.MouseDragging.Drop;
using Melville.MVVM.Wpf.WpfHacks;

namespace Melville.MVVM.Wpf.MouseDragging.ListRearrange
{
    public sealed class TreeDropDriver
    {
        #region Construction and Binding

        private readonly FrameworkElement rootElt;
        private readonly Type dropType;
        private string DragTypeName() => dropType.FullName??"";

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
            TreeArrange.GetVisualToDrag(fe) is {} dragTarget
                ? new MouseDragger(dragTarget, e).LeafTarget()
                : new MouseDragger(fe, e).TypedTarget(typeof(ListViewItem), typeof(TreeViewItem), typeof(ListBoxItem));

        private static object? FindDraggedItem(FrameworkElement fe) => 
            TreeArrange.GetDraggedItem(fe) ?? fe.DataContext;

        private DataObject GetDataObject(FrameworkElement fe)
        {
            var data = FindDraggedItem(fe);
            if (data == null) return new DataObject();
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

        private IDropTarget? SupplementalDropTarget() => TreeArrange.GetSupplementalDropTarget(rootElt);

        private FrameworkElement? AdornmentTarget(object elt)
        {
            return elt switch
            {
                DependencyObject dobj when TreeArrange.GetVisualToDrag(dobj) is { } explicitTarget => explicitTarget,
                FrameworkElement fe => TryGetContainingCollectionViewItem(fe),
                _ => null
            };
        }

        private static FrameworkElement TryGetContainingCollectionViewItem(FrameworkElement fe) =>
            DependencyObjectExtensions.Parents(fe)
                .OfType<FrameworkElement>()
                .FirstOrDefault(i => i is ListViewItem || i is TreeViewItem || i is ListBoxItem) ?? fe;


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

        private IList? FindList(FrameworkElement targetElement, object targetData) =>
            targetElement.Parents().OfType<ItemsControl>()
                .Select(i => i.ItemsSource)
                .OfType<IList>()
                .FirstOrDefault(i => i.Contains(targetData));

        #endregion
    }
}