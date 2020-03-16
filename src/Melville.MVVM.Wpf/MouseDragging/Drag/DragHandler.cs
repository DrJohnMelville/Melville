using  System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Melville.MVVM.Wpf.WpfHacks;

namespace Melville.MVVM.Wpf.MouseDragging.Drag
{
  public class DragHandler
  {
    private readonly IMouseDataSource source;
    private readonly LocalDragEventArgs mouseDownArgs;

    public DragHandler(IMouseDataSource source, LocalDragEventArgs mouseDownArgs)
    {
      this.source = source;
      this.mouseDownArgs = mouseDownArgs;
    }

    public DragDropEffects InitiateDrag(IDataObject dataToDrag, DragDropEffects allowedEffects)
    {
      DragDrop.AddGiveFeedbackHandler(mouseDownArgs.Target, GiveFeedback);
      var ret=DragDrop.DoDragDrop(mouseDownArgs.Target, dataToDrag, allowedEffects);
      DragDrop.RemoveGiveFeedbackHandler(mouseDownArgs.Target, GiveFeedback);
      RaiseMouseUpMethod();
      return ret;
    }

    //Drag and drop blocks the mouse move and up events, so we will simulate them so the entire
    // local drag framework still works.

    private void RaiseMouseUpMethod()
    {
      var ea = new MouseEventArgs(Mouse.PrimaryDevice, Environment.TickCount);
      source.Root.SendMousePosition(MouseMessageType.Up, ea.GetPosition(mouseDownArgs.Target));
      source.Root.CancelMouseBinding();
    }

    private void GiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
      var ea = new MouseEventArgs(Mouse.PrimaryDevice, Environment.TickCount);
      source.Root.SendMousePosition(MouseMessageType.Move, ea.GetPosition(mouseDownArgs.Target));
      e.UseDefaultCursors = true;
      e.Handled = true;
    }
 }
}