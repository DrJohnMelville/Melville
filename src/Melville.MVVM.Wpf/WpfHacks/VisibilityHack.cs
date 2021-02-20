using System.Windows;
using Melville.INPC;

namespace Melville.MVVM.Wpf.WpfHacks
{
    public static partial class VisibilityHack
    {
        private static void ComputeVisibility(DependencyObject d, bool show, Visibility invisibleState)
        {
            if (d is UIElement i)
            {
                i.Visibility = show ? invisibleState : Visibility.Visible;
            }
        }

        [GenerateDP]
        private static void OnCollapseIfChanged(DependencyObject d, bool show = false) => 
            ComputeVisibility(d, show, Visibility.Collapsed);

        [GenerateDP]
        private static void OnHideIfChanged(DependencyObject d, bool show = false) =>
            ComputeVisibility(d, show, Visibility.Hidden);
  
        [GenerateDP]
        private static void OnCollapseUnlessChanged(DependencyObject d, bool show = true) =>
            OnCollapseIfChanged(d, !show);
        [GenerateDP]
        private static void OnHideUnlessChanged(DependencyObject d, bool show = true) =>
            OnHideIfChanged(d, !show);

        [GenerateDP(Default = "ImplausibleString kgjk;h&^#$jhc;")]
        private static void OnCollapseIfNullChanged(DependencyObject d, object? arg) =>
            OnCollapseIfChanged(d, arg == null);
        [GenerateDP(Default = "ImplausibleString kgjk;h&^#$jhc;")]
        private static void OnCollapseUnlessNullChanged(DependencyObject d, object? arg) =>
            OnCollapseIfChanged(d, arg != null);
        [GenerateDP(Default = "ImplausibleString kgjk;h&^#$jhc;")]
        private static void OnCollapseIfWhitespaceChanged(DependencyObject d, object? arg) =>
            OnCollapseIfChanged(d, string.IsNullOrWhiteSpace(arg?.ToString()));
        [GenerateDP(Default = "ImplausibleString kgjk;h&^#$jhc;")]
        private static void OnCollapseUnlessWhitespaceChanged(DependencyObject d, object? arg) =>
            OnCollapseIfChanged(d, !string.IsNullOrWhiteSpace(arg?.ToString()));
    }
}