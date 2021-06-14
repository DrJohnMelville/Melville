using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Melville.INPC;
using Melville.MVVM.Wpf.MouseDragging.Adorners;

namespace Melville.MVVM.Wpf.MouseDragging.Drop
{
  public static partial class DropBinding
  {
    public static void ClearAdorners(this Visual target)
    {
      oldLayer?.Remove(oldAdorner);
      oldLayer = null;
    }

    public static Adorner[] GetAdorners(this FrameworkElement fe) => oldAdorner == null ? new Adorner[0]: new []{oldAdorner};

    public static void Adorn(this FrameworkElement target, DropAdornerKind adorner) =>
      target.Adorn(DropAdornerFactory.Create(adorner, target));

    public static void Adorn(this UIElement target, Adorner adorner)
    {
      oldAdorner = adorner;
      oldLayer = AdornerLayer.GetAdornerLayer(target);
      oldLayer.Add(oldAdorner);
    }

    private static AdornerLayer? oldLayer = null;
    private static Adorner? oldAdorner = null;
    
    [GenerateDP(Attached = true)]
    public static void OnDropMethodChanged(FrameworkElement target, string dropName)
    {
      SetupDropMethod(target, dropName);
    }
    [GenerateDP(Attached = true)]
    public static void OnDropWithDragMethodChanged(FrameworkElement target, string dropName)
    {
      SetupDropMethod(target, dropName+"?");
    }
    private static void SetupDropMethod(FrameworkElement target, string TargetName)
    {
      var match = Regex.Match(TargetName, @"^(\w+)([\?\!]*)$");
      new DropTarget(target, match.Groups[1].Value).BindToTargetControl(
        match.Groups[2].Value.Contains("?"),
        match.Groups[2].Value.Contains("!"));
    }
  }
}