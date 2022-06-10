using System;
using System.Windows;
using Melville.INPC;
using Melville.MVVM.Wpf.EventBindings.SearchTree;

namespace Melville.MVVM.Wpf.MouseDragging.Drop;

public interface IDropTarget
{
    void DragOver(object sender, DragEventArgs e);
    void DragLeave(object sender, DragEventArgs e);
    void DragEnter(object sender, DragEventArgs e);
    void HandleDrop(object sender, DragEventArgs e);
}

public partial class DropTarget : IDropTarget
{
    [FromConstructor] private readonly FrameworkElement target;
    [FromConstructor] private readonly string method;
    
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
        
    public void DragLeave(object sender, DragEventArgs e) { }

    public void HandleDrop(object sender, DragEventArgs e) => HandleQueryOrDrop(new DropAction(e, target), e);

    private void HandleQueryOrDrop(IDropInfo adapter, DragEventArgs e)
    {
        if (!new VisualTreeRunner(target).RunTreeSearch(method, new object?[] {adapter}, out var result)) return;
        e.Handled = true;
        e.Effects = result as DragDropEffects? ?? DragDropEffects.None;
    }
}