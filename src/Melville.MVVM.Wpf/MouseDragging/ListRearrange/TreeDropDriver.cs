using System;
using System.Collections;
using System.Linq;
using System.Windows;
using Melville.INPC;
using Melville.MVVM.Wpf.MouseDragging.Adorners;
using Melville.MVVM.Wpf.MouseDragging.Drop;
using Melville.MVVM.Wpf.WpfHacks;
using Microsoft.VisualBasic.Devices;

namespace Melville.MVVM.Wpf.MouseDragging.ListRearrange;

public sealed partial class TreeDropDriver
{
    [FromConstructor]private readonly FrameworkElement rootElt;
    [FromConstructor]private readonly Type dropType;
    private string DragTypeName() => dropType.FullName??"";


    public void AttachEvents(FrameworkElement elt) => 
        new HierarchicalDropWithDragBinder(elt, true, DragOver, DragOver, DragLeave, Drop);

        
    #region Adorn on drag

    private void DragLeave(object sender, DragEventArgs e)
    {
        if (SupplementalDropTarget() is IDropTarget target)
        {
            target.DragLeave(sender, e);
        }
    }

    private void DragOver(object sender, DragEventArgs e)
    {
        if (TreeArrange.AdornmentTarget(sender) is {} droppedOnElement)
        {
            if (ExtractDraggedData(e) is not {} droppedData)
            {
                DelegateDragOverToSupplementalDrag(sender, e);
                return;
            };

            double relativePosition = RelativePosition(e, droppedOnElement);
            droppedOnElement.Adorn(ComputeAdornerType(relativePosition, HasChildList(droppedOnElement, droppedData)));
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }
    }

    private DropAdornerKind ComputeAdornerType(double relativePosition, bool hasChildList) =>
        (hasChildList, relativePosition) switch
        {
            (true, > 0.25 and < 0.75) => DropAdornerKind.Rectangle,
            (_, <= 0.5) => DropAdornerKind.Top,
            _ => DropAdornerKind.Bottom
        };

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


    private static double RelativePosition(DragEventArgs e, FrameworkElement fe) => 
        e.GetPosition(fe).Y / fe.ActualHeight;

    #endregion

    #region Drop

    private void Drop(object sender, DragEventArgs e)
    {
        if (TreeArrange.AdornmentTarget(sender) is not { } droppedOnElement) return;
        if (ExtractDraggedData(e) is not {} draggedItem)
        {
            TrySendSupplementalDropMessage(sender, e);
            return;
        }
        if (TreeArrange.FindDraggedItem(droppedOnElement) is not {} target || target == draggedItem)  return;
        if (DroppingItemOnOwnChild(droppedOnElement, draggedItem)) return;
        if (ListFinder.FindParentListContainingData(droppedOnElement, target) is not {} items) return;
        e.Handled = true;

            
        e.Effects = ComputeAdornerType(RelativePosition(e, droppedOnElement), HasChildList(droppedOnElement, draggedItem)) switch
        {
            DropAdornerKind.Top => InsertDroppedItemIntoTarget(items, target, draggedItem, 0),
            DropAdornerKind.Bottom => InsertDroppedItemIntoTarget(items, target, draggedItem, 1),
            _ => InsertDroppedItemIntoTarget(
                ListFinder.FindChildListToHoldData(droppedOnElement, draggedItem) ?? items, null, draggedItem, 0)
        };
    }

    private static bool DroppingItemOnOwnChild(FrameworkElement droppedOnElement, object draggedItem) =>
        droppedOnElement.Parents()
            .Any(i => (i is FrameworkElement fe && fe.DataContext == draggedItem));

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