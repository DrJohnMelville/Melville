using  System;
using System.Windows;
using System.Windows.Documents;
using Melville.MVVM.Wpf.SearchTree;
using Melville.MVVM.Wpf.MouseDragging.Adorners;
using Serilog;

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

    public void BindToTargetControl(bool monitorDragContinue)
    {
      target.AllowDrop = true;
      target.Drop += HandleDrop;
      target.DragEnter += DragEnter;
      target.DragLeave += DragLeave;
      if (monitorDragContinue)
      {
        target.DragOver += DragOver;
      }
      else
      {
        target.DragOver += RepeatEntryDetermination;
      }
    }

    public void DragOver(object sender, DragEventArgs e)
    {
      DropBinding.ClearAdorners(target);
      DragEnter(sender, e);
    }

    public void DragLeave(object sender, DragEventArgs e)
    {
      DropBinding.ClearAdorners(target);
    }

    private DragDropEffects entryEffect = DragDropEffects.None;

    public void DragEnter(object sender, DragEventArgs e)
    {
      HandleQueryOrDrop(new DropQuery(e, target), e);
      entryEffect = e.Effects;
    }

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
      if (RunOnVisualTreeSearch.Run(target, method, new[] { adapter }, out var result))
      {
        e.Handled = true;
        e.Effects = result as DragDropEffects? ?? DragDropEffects.None;
      }
    }
  }

  public static class DropBinding
  {
    public static void ClearAdorners(this FrameworkElement target)
    {
      //      var adornerLayer = AdornerLayer.GetAdornerLayer(target);
      //      if (adornerLayer == null) return`;
      //      foreach (var adorner in adornerLayer.GetAdorners(target) ?? new Adorner[0])
      //      {
      //        adornerLayer.Remove(adorner);
      //      }

      oldLayer?.Remove(oldAdorner);
      oldLayer = null;
    }

    public static Adorner[] GetAdorners(this FrameworkElement fe) => oldAdorner == null ? new Adorner[0]: new []{oldAdorner};

    public static void Adorn(this FrameworkElement target, DropAdornerKind adorner) =>
      target.Adorn(DropAdornerFactory.Create(adorner, target));

    public static void Adorn(this FrameworkElement target, Adorner adorner)
    {
      oldAdorner = adorner;
      oldLayer = AdornerLayer.GetAdornerLayer(target);
      oldLayer.Add(oldAdorner);
    }

    private static AdornerLayer? oldLayer = null;
    private static Adorner? oldAdorner = null;


    public static string GetDropMethod(DependencyObject obj) => (string)obj.GetValue(DropMethodProperty);
    public static void SetDropMethod(DependencyObject obj, string value) => obj.SetValue(DropMethodProperty, value);
    public static readonly DependencyProperty DropMethodProperty =
        DependencyProperty.RegisterAttached("DropMethod", typeof(string), typeof(DropBinding), 
          new PropertyMetadata(null, RegisterDrop));

    private static void RegisterDrop(DependencyObject d, DependencyPropertyChangedEventArgs e) => InnerRegisterDrop(d, e, false);

    private static void InnerRegisterDrop(DependencyObject d, DependencyPropertyChangedEventArgs e, bool monitorDragContinue)
    {
      if (e.NewValue == null) return;
      new DropTarget((FrameworkElement) d, e.NewValue.ToString()??"").BindToTargetControl(monitorDragContinue);
    }

    public static string GetDropWithDragMethod(DependencyObject obj) => (string)obj.GetValue(DropWithDragMethodProperty);
    public static void SetDropWithDragMethod(DependencyObject obj, string value) => obj.SetValue(DropWithDragMethodProperty, value);
    public static readonly DependencyProperty DropWithDragMethodProperty =
        DependencyProperty.RegisterAttached("DropWithDragMethod", typeof(string), typeof(DropBinding), 
          new PropertyMetadata(null, RegisterDropWithDrag));

    private static void RegisterDropWithDrag(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
      InnerRegisterDrop(d, e, true);

  }
}