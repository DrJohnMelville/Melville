using Melville.INPC;
using Microsoft.Maui.Controls;

namespace Melville.MVVM.Maui.Hacks;

public static partial class VisibilityHack
{
    
    private static void ComputeVisibility(BindableObject d, bool hide)
    {
        if (d is VisualElement i)
        {
            i.IsVisible = !hide;
        }
    }

    public static readonly global::Microsoft.Maui.Controls.BindableProperty CollapseIfProp2erty = 
        global::Microsoft.Maui.Controls.BindableProperty.CreateAttached(
            "CollapseIf", typeof(bool), typeof(Melville.MVVM.Maui.Hacks.VisibilityHack),
            false, propertyChanged: (i,j,k)=>global::Melville.MVVM.Maui.Hacks.VisibilityHack.OnCollapseIfChanged(i,(bool)k));

    [GenerateBP]
    private static void OnCollapseIfChanged(BindableObject d, bool hide = false) => 
        ComputeVisibility(d, hide);

    [GenerateBP]
    private static void OnCollapseUnlessChanged(BindableObject d, bool show = true) =>
        OnCollapseIfChanged(d, !show);
    [GenerateBP(Default = "ImplausibleString kgjk;h&^#$jhc;")]
    private static void OnCollapseIfNullChanged(BindableObject d, object? arg) =>
        OnCollapseIfChanged(d, arg == null);
    [GenerateBP(Default = "ImplausibleString kgjk;h&^#$jhc;")]
    private static void OnCollapseUnlessNullChanged(BindableObject d, object? arg) =>
        OnCollapseIfChanged(d, arg != null);
    [GenerateBP(Default = "ImplausibleString kgjk;h&^#$jhc;")]
    private static void OnCollapseIfWhitespaceChanged(BindableObject d, object? arg) =>
        OnCollapseIfChanged(d, string.IsNullOrWhiteSpace(arg?.ToString()));
    [GenerateBP(Default = "ImplausibleString kgjk;h&^#$jhc;")]
    private static void OnCollapseUnlessWhitespaceChanged(BindableObject d, object? arg) =>
        OnCollapseIfChanged(d, !string.IsNullOrWhiteSpace(arg?.ToString()));
}