using  System;
using System.Windows;
using System.Windows.Input;
using Melville.INPC;

namespace Melville.MVVM.Wpf.MouseDragging.Drag;

public partial class DragHandler
{
  [FromConstructor]private readonly IMouseDataSource source;

  public DragDropEffects InitiateDrag(IDataObject dataToDrag, DragDropEffects allowedEffects)
  {
    if (source.Target is not FrameworkElement src) return DragDropEffects.None;
    DragDrop.AddGiveFeedbackHandler(src, GiveFeedback);
    var ret=DragDrop.DoDragDrop(src, dataToDrag, allowedEffects);
    DragDrop.RemoveGiveFeedbackHandler(src, GiveFeedback);
    RaiseMouseUpMethod(src);
    return ret;
  }

  //Drag and drop blocks the mouse move and up events, so we will simulate them so the entire
  // local drag framework still works.

  private void RaiseMouseUpMethod(FrameworkElement target)
  {
    var ea = new MouseEventArgs(Mouse.PrimaryDevice, Environment.TickCount);
    source.SendMousePosition(MouseMessageType.Up, ea.GetPosition(target));
    source.CancelMouseBinding();
  }

  private void GiveFeedback(object sender, GiveFeedbackEventArgs e)
  {
    if (sender is not FrameworkElement target) return;
    var ea = new MouseEventArgs(Mouse.PrimaryDevice, Environment.TickCount);
    source.SendMousePosition(MouseMessageType.Move, ea.GetPosition(target));
    e.UseDefaultCursors = true;
    e.Handled = true;
  }
}