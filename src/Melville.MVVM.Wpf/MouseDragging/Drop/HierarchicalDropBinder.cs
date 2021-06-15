using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.Drop
{
    public class HierarchicalDropWithDragBinder: HierarchicalDropBinder
    {
        private readonly DragEventHandler over;
        public HierarchicalDropWithDragBinder(UIElement elt, bool preview, 
            DragEventHandler enter, DragEventHandler over, DragEventHandler leave, DragEventHandler drop) : 
            base(elt, preview, enter, leave, drop)
        {
            this.over = over;
        }
        protected override void HandleOver(object sender, DragEventArgs e)
        {
            if (MayHavePriorAdorner()) Elt.ClearAdorners();
            if (ChildHandledDrop(e)) return;
            over(sender, e);
            LastDragEffect = e.Effects;
        }
    }
    public class HierarchicalDropBinder
    {
        protected DragDropEffects LastDragEffect { get; set; } = DragDropEffects.None;
        protected UIElement Elt { get; }
        private readonly DragEventHandler enter;
        private readonly DragEventHandler leave;
        private readonly DragEventHandler drop;

        public HierarchicalDropBinder(UIElement elt, bool preview, 
            DragEventHandler enter, DragEventHandler leave, DragEventHandler drop)
        {
            Elt = elt;
            this.enter = enter;
            this.leave = leave;
            this.drop = drop;

            BindMethods(preview);
        }

        private void BindMethods(bool preview)
        {
            Elt.AllowDrop = true;
            Elt.AddHandler(UIElement.DragEnterEvent, new DragEventHandler(HandleEnter), preview);
            Elt.AddHandler(UIElement.DragOverEvent, new DragEventHandler(HandleOver), preview);
            Elt.AddHandler(UIElement.DragLeaveEvent, new DragEventHandler(HandleLeave), preview);
            Elt.AddHandler(UIElement.DropEvent, new DragEventHandler(HandleDrop), preview);        }

        private void HandleEnter(object sender, DragEventArgs e)
        {
            if (ChildHandledDrop(e)) return;
            enter(sender, e);
            LastDragEffect = e.Effects;
        }

        protected  virtual void HandleOver(object sender, DragEventArgs e)
        {
            if (ChildHandledDrop(e)) return;
            e.Handled = true;
            e.Effects = LastDragEffect;
        }

        private void HandleLeave(object sender, DragEventArgs e)
        {
            if (!MayHavePriorAdorner()) return;
            leave(sender, e);
            Elt.ClearAdorners();
        }

        private void HandleDrop(object sender, DragEventArgs e)
        {
            if (!MayHavePriorAdorner()) return;
            drop(sender, e);
            Elt.ClearAdorners();
            LastDragEffect = DragDropEffects.None;
        }

        protected static bool ChildHandledDrop(DragEventArgs e) => 
            e.Handled && e.Effects != DragDropEffects.None;

        protected bool MayHavePriorAdorner() => LastDragEffect != DragDropEffects.None;
    }
}