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
            if (monitorDragContinue)
                new HierarchicalDropWithDragBinder(target, preview, DragEnter, DragOver, DragLeave, HandleDrop);
            else
               new HierarchicalDropBinder(target, preview, DragEnter, DragLeave, HandleDrop);
        }
        
        public void DragEnter(object sender, DragEventArgs e)
        {
            HandleQueryOrDrop(new DropQuery(e, target), e);
        }
        public void DragOver(object sender, DragEventArgs e)
        {
            DragEnter(sender, e);
        }
        
        public void DragLeave(object sender, DragEventArgs e)
        {
        }

        public void HandleDrop(object sender, DragEventArgs e)
        { 
            HandleQueryOrDrop(new DropAction(e, target), e);
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