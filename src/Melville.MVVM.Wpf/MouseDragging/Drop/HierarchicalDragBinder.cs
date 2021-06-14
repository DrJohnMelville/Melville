using System;
using System.Reflection;
using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.Drop
{
    public class HierarchicalDragBinder
    {
        private DragDropEffects lastDragEffect = DragDropEffects.None;
        private readonly UIElement elt;
        private readonly DragEventHandler enter;
        private readonly DragEventHandler over;
        private readonly DragEventHandler leave;
        private readonly DragEventHandler drop;

        public HierarchicalDragBinder(UIElement elt, bool preview,
            DragEventHandler enter, DragEventHandler leave, DragEventHandler drop) :
            this(elt, preview, enter, enter, leave, drop)
        {
        }

        public HierarchicalDragBinder(UIElement elt, bool preview, 
            DragEventHandler enter, DragEventHandler over, DragEventHandler leave, DragEventHandler drop)
        {
            this.elt = elt;
            this.enter = enter;
            this.over = over;
            this.leave = leave;
            this.drop = drop;

            BindMethods(preview);
        }

        private void BindMethods(bool preview)
        {
            elt.AddHandler(UIElement.DragEnterEvent, new DragEventHandler(HandleEnter), preview);
            elt.AddHandler(UIElement.DragOverEvent, new DragEventHandler(HandleOver), preview);
            elt.AddHandler(UIElement.DragLeaveEvent, new DragEventHandler(HandleLeave), preview);
            elt.AddHandler(UIElement.DropEvent, new DragEventHandler(HandleDrop), preview);        }

        private void HandleEnter(object sender, DragEventArgs e)
        {
            if (ChildHandledDrop(e)) return;
            over(sender, e);
            lastDragEffect = e.Effects;
        }

        private void HandleOver(object sender, DragEventArgs e)
        {
            if (MayHavePriorAdorner()) elt.ClearAdorners();
            if (ChildHandledDrop(e)) return;
            over(sender, e);
            lastDragEffect = e.Effects;
        }

        private void HandleLeave(object sender, DragEventArgs e)
        {
            if (!MayHavePriorAdorner()) return;
            leave(sender, e);
            elt.ClearAdorners();
        }

        private void HandleDrop(object sender, DragEventArgs e)
        {
            if (!MayHavePriorAdorner()) return;
            drop(sender, e);
            lastDragEffect = DragDropEffects.None;
        }
        
        private static bool ChildHandledDrop(DragEventArgs e) => 
            e.Handled && e.Effects != DragDropEffects.None;

        private bool MayHavePriorAdorner() => lastDragEffect != DragDropEffects.None;
    }
}