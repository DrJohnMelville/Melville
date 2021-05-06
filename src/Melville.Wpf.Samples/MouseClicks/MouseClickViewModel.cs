using System.Windows;
using System.Windows.Input;
using ABI.Windows.Foundation.Collections;
using Melville.INPC;
using Melville.MVVM.Wpf.MouseClicks;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.Adorners;
using Melville.MVVM.Wpf.MouseDragging.Drag;
using Melville.MVVM.Wpf.MouseDragging.Drop;

namespace Melville.Wpf.Samples.MouseClicks
{
    public sealed partial class MouseClickViewModel
    {
        [AutoNotify] private string message = "Mouse Clicks";
        [AutoNotify] private string dropAreaText = "Drop Area";
        public void MouseDownHandler(IMouseClickReport report)
        { 
            Message = $"Clicked ({report.AbsoluteLocation()}) / ({report.RelativeLocation()})";
        }

        public void BeginDrag(IMouseClickReport click)
        {
            click.CreateDragger()
                .LeafTarget()
                .DragTarget(0.5)
                .Drag(()=>new DataObject("This is Dragged Text"), DragDropEffects.Copy);
        }

        public DragDropEffects JdmDrop(IDropQuery info)
        {
            if (!info.Item.GetDataPresent(typeof(string))) return DragDropEffects.None;
            info.AdornTarget(DropAdornerKind.Rectangle);
            DropAreaText = $"{info.GetTargetLocation()} ==> {info.GetRelativeTargetLocation()}";
            return DragDropEffects.All;
        }
        public void JdmDrop(IDropAction info)
        {
            DropAreaText = info.Item.GetString()??"No Data";
        }
    }
}