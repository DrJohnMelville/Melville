using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Melville.MVVM.Wpf.MouseClicks;
using Melville.MVVM.Wpf.MouseDragging.Adorners;
using Melville.MVVM.Wpf.MouseDragging.Drag;
using Melville.MVVM.Wpf.MouseDragging.Drop;
using Melville.MVVM.Wpf.VisualTreeLocations;
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
                    if (operation == DragDropEffects.Move && FindDraggedItem(fe) is {} item)
                      ListFinder.FindParentListContainingData(fe, item)?.Remove(fe.DataContext);
                }
            }
        }

        private static IMouseDataSource CreateDragger(MouseButtonEventArgs e, FrameworkElement fe) =>
            TreeArrange.GetVisualToDrag(fe) is {} dragTarget
                ? new MouseClickReport(dragTarget, e).DragSource()
                : new MouseClickReport(fe, e)
                    .AttachToDataContextHolder(typeof(ListViewItem), typeof(TreeViewItem), typeof(ListBoxItem))
                    .DragSource();

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
            if (AdornmentTarget(sender) is {} droppedOnElement)
            {
                droppedOnElement.ClearAdorners();
                if (ExtractDraggedData(e) is not {} droppedData)
                {
                    DelegateDragOverToSupplementalDrag(sender, e);
                    return;
                };

                double relativePosition = RelativePosition(e, droppedOnElement);
                droppedOnElement.Adorn(ComputeAdornerType(droppedOnElement, droppedData, relativePosition));

                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
        }

        private DropAdornerKind ComputeAdornerType(FrameworkElement droppedOnElement, object droppedData, double relativePosition) =>
            HasChildList(droppedOnElement, droppedData)
                ? DropTypeByPositionThreeWay(relativePosition)
                : DropTypeByPosition(relativePosition);

        private static bool HasChildList(FrameworkElement droppedOnElement, object droppedData) => 
            ListFinder.FindChildListToHoldData(droppedOnElement, droppedData) is not null;

        private bool DelegateDragOverToSupplementalDrag(object sender, DragEventArgs e)
        {
            if (SupplementalDropTarget() is {} target)
            {
                target.DragOver(sender, e);
                return true;
            }

            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return false;
        }

        private IDropTarget? SupplementalDropTarget() => TreeArrange.GetSupplementalDropTarget(rootElt);

        private FrameworkElement? AdornmentTarget(object elt) => elt switch
            {
                DependencyObject dobj when TreeArrange.GetVisualToDrag(dobj) is { } explicitTarget => explicitTarget,
                FrameworkElement fe => TryGetContainingCollectionViewItem(fe),
                _ => null
            };

        private static FrameworkElement TryGetContainingCollectionViewItem(FrameworkElement fe) =>
            DependencyObjectExtensions.Parents(fe)
                .OfType<FrameworkElement>()
                .FirstOrDefault(i => i is ListViewItem || i is TreeViewItem || i is ListBoxItem) ?? fe;


        private static double RelativePosition(DragEventArgs e, FrameworkElement fe) => 
            e.GetPosition(fe).Y / fe.ActualHeight;

        private static DropAdornerKind DropTypeByPosition(double relativePosition) =>
            relativePosition > 0.5 ? DropAdornerKind.Bottom : DropAdornerKind.Top;

        private DropAdornerKind DropTypeByPositionThreeWay(double relativePosition) =>
            relativePosition switch
            {
                <= 0.25 => DropAdornerKind.Top,
                >= 0.75 => DropAdornerKind.Bottom,
                _ => DropAdornerKind.Rectangle
            };


        #endregion

        #region Drop

        private void Drop(object sender, DragEventArgs e)
        {
            if (AdornmentTarget(sender) is not { } droppedOnElement) return;
            droppedOnElement.ClearAdorners();
            if (ExtractDraggedData(e) is not {} draggedItem)
            {
                TrySendSupplementalDropMessage(sender, e);
                return;
            }
            if (FindDraggedItem(droppedOnElement) is not {} target || target == draggedItem)  return;
            if (ListFinder.FindParentListContainingData(droppedOnElement, target) is not {} items) return;
            e.Handled = true;

            e.Effects = ComputeAdornerType(droppedOnElement, draggedItem, RelativePosition(e, droppedOnElement)) switch
            {
                DropAdornerKind.Top => InsertDroppedItemIntoTarget(items, target, draggedItem, 0),
                DropAdornerKind.Bottom => InsertDroppedItemIntoTarget(items, target, draggedItem, 0),
                _ => InsertDroppedItemIntoTarget(
                    ListFinder.FindChildListToHoldData(droppedOnElement, draggedItem) ?? items, null, draggedItem, 0)
            };
        }

        private object? ExtractDraggedData(DragEventArgs e) => e.Data.GetData(DragTypeName());

        private void TrySendSupplementalDropMessage(object sender, DragEventArgs e) => 
            SupplementalDropTarget()?.HandleDrop(sender, e);

        private static DragDropEffects TryRemoveSourceFromTargetList(IList items, object? draggedItem)
        {
            if (!items.Contains(draggedItem)) return DragDropEffects.Move;
            items.Remove(draggedItem);
            return DragDropEffects.Copy;
        }
        
        private static DragDropEffects InsertDroppedItemIntoTarget(IList items, object? target,
            object draggedItem, int dropItemPositionDelta)
        {
            var ret = TryRemoveSourceFromTargetList(items, draggedItem);
            // cannot lift index computation out of this method because it has to be after the target may be removed from the list
            items.Insert(ComputeTargetLocation(items, target, dropItemPositionDelta), draggedItem);
            return ret;
        }

        private static int ComputeTargetLocation(IList items, object? target, int dropItemPositionDelta) => 
            target == null? 
                items.Count: 
                items.IndexOf(target) + (dropItemPositionDelta);

        #endregion
    }
}