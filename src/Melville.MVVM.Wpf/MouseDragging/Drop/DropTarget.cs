using System;
using System.Windows;
using Melville.MVVM.Wpf.EventBindings.SearchTree;

namespace Melville.MVVM.Wpf.MouseDragging.Drop
{
    public interface IDropTarget
    {
        void DragOver(object sender, DragEventArgs e);
        void DragLeave(object sender, DragEventArgs e);
        void DragEnter(object sender, DragEventArgs e);
        void HandleDrop(object sender, DragEventArgs e);
    }


    public class DropTarget : IDropTarget
    {
        private readonly FrameworkElement target;
        private readonly string method;

        public DropTarget(FrameworkElement target, string method)
        {
            this.target = target;
            this.method = method;
        }

        public void BindToTargetControl(bool monitorDragContinue, bool preview)
        {
            target.AllowDrop = true;
            target.AddHandler(UIElement.DropEvent, Wrap(HandleDrop, preview), preview);
            target.AddHandler(UIElement.DragEnterEvent, Wrap(DragEnter, preview), preview);
            target.AddHandler(UIElement.DragLeaveEvent, Wrap(DragLeave, preview), preview);
            target.AddHandler(UIElement.DragOverEvent, Wrap(SelectDragOver(monitorDragContinue), preview),
                preview);
        }

        private DragEventHandler SelectDragOver(bool monitorDragContinue)
        {
            return (monitorDragContinue) ? DragOver : RepeatEntryDetermination;
        }

        private DragEventHandler Wrap(DragEventHandler handleDrop, bool preview)
        {
            return preview ?
                (s,e)=>
                {
                    if (!e.Handled || (e.Effects == DragDropEffects.None)) handleDrop(s, e);
                }:
                handleDrop;
        }

        public void DragEnter(object sender, DragEventArgs e)
        {
            HandleQueryOrDrop(new DropQuery(e, target), e);
            entryEffect = e.Effects;
        }
        public void DragOver(object sender, DragEventArgs e)
        {
            ClearAdornersSetByThisObject();
            DragEnter(sender, e);
        }

        private void ClearAdornersSetByThisObject()
        {
            if (PriorMoveAdornedItem())
            {
                DropBinding.ClearAdorners(target);
            }
        }

        // if we are getting update events when we are not dragging, it must mean that
        // if an outer dragger is dragging and we need to get out of the way
        private bool PriorMoveAdornedItem() => entryEffect != DragDropEffects.None;

        public void DragLeave(object sender, DragEventArgs e)
        {
            DropBinding.ClearAdorners(target);
            entryEffect = DragDropEffects.None;
        }

        private DragDropEffects entryEffect = DragDropEffects.None;


        /// <summary>
        /// To set the mouse icon correctly, I have to respond to this message and update the effects.
        /// For elements that have the some drop behavior evewhere, I do not need to recompute adorners
        /// or droppability with each mouse move.  I do, though have to parrot back what the enter routine determined
        /// was the correct value or it defaults to showing a copy icon.
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used.</param>
        private void RepeatEntryDetermination(object sender, DragEventArgs e)
        {
            e.Effects = entryEffect;
            e.Handled = true;
        }

        public void HandleDrop(object sender, DragEventArgs e)
        { 
            HandleQueryOrDrop(new DropAction(e, target), e);
            DropBinding.ClearAdorners(target);
        }

        private void HandleQueryOrDrop(IDropInfo adapter, DragEventArgs e)
        {
            object?[] inputParams = new[] { adapter };
            if (new VisualTreeRunner(target).RunTreeSearch(method, inputParams, out var result))
            {
                e.Handled = true;
                e.Effects = result as DragDropEffects? ?? DragDropEffects.None;
            }
        }
    }
}