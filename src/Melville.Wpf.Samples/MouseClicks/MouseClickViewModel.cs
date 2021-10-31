using System.Windows;
using System.Windows.Input;
using ABI.Windows.Foundation.Collections;
using Melville.INPC;
using Melville.MVVM.Wpf.KeyboardFacade;
using Melville.MVVM.Wpf.MouseClicks;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.Adorners;
using Melville.MVVM.Wpf.MouseDragging.Drag;
using Melville.MVVM.Wpf.MouseDragging.Drop;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;

namespace Melville.Wpf.Samples.MouseClicks;

public sealed partial class MouseClickViewModel
{
    [AutoNotify] private string message = "Mouse Clicks";
    [AutoNotify] private string dropAreaText = "Drop Area";
    [AutoNotify] private string keyText = "____";
    public void MouseDownHandler(IMouseClickReport report)
    { 
        Message = $"Clicked ({report.AbsoluteLocation()}) / ({report.RelativeLocation()})";
        report
            .ExtractSize(out var size)
            .DragSource()
            .BindLocalDragger(
                LocalDragger.MinimumDrag(
                    LocalDragger.RelativeToSize(size,
                        LocalDragger.Action(p=>Message = $"Drag to {p}"))));
            
    }

    public void BeginDrag(IMouseClickReport click)
    {
        click.DragSource()
            .DragTarget(0.5)
            .Drag(()=>new DataObject("This is Dragged Text"), DragDropEffects.Copy);
    }
    public void BeginDragInt(IMouseClickReport click)
    {
        click.DragSource()
            .DragTarget(0.5)
            .Drag(()=>new DataObject(12), DragDropEffects.Copy);
    }

    public DragDropEffects Drop(IDropQuery info)
    {
        if (!info.Item.GetDataPresent(typeof(string))) return DragDropEffects.None;
        info.AdornTarget(DropAdornerKind.Rectangle);
        DropAreaText = $"{info.GetTargetLocation()} ==> {info.GetRelativeTargetLocation()}";
        return DragDropEffects.All;
    }

    public DragDropEffects Drop(IDropAction info)
    {
        var str =  "";
        return info.AcceptDrop((str = info.Item.GetString())!= null, DragDropEffects.Copy, 
            ()=>DropAreaText = $"Dragged a string {str}");
    }

    public DragDropEffects DropInt(IDropAction info)
    {
        return info.AcceptDrop<int>(DragDropEffects.Copy, number =>DropAreaText = $"Dragged an int {number}"); 
    }

    public DragDropEffects DropInt(IDropQuery info)
    {
        return info.AdornIfType<int>(DragDropEffects.All);
    }

    public void MapKeyDown(IKeyEventReport keyEvent)
    {
        KeyText = keyEvent.Key.ToString();
    }
}