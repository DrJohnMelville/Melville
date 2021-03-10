using Melville.INPC;

namespace Melville.MVVM.Wpf.EventBindings.SearchTree
{
    [MacroItem("T1")]
    [MacroItem("T1, T2")]
    [MacroItem("T1, T2, T3")]
    [MacroItem("T1, T2, T3, T4")]
    [MacroItem("T1, T2, T3, T4, T5")]
    [MacroItem("T1, T2, T3, T4, T5, T6")]
    [MacroItem("T1, T2, T3, T4, T5, T6, T7")]
    [MacroItem("T1, T2, T3, T4, T5, T6, T7, T8")]
    [MacroItem("T1, T2, T3, T4, T5, T6, T7, T8, T9")]
    [MacroItem("T1, T2, T3, T4, T5, T6, T7, T8, T9, T10")]
    [MacroItem("T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12")]
    [MacroItem("T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13")]
    [MacroItem("T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14")]
    [MacroItem("T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14, T15")]
    [MacroItem("T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14, T15, T16")]
    [MacroCode(@"public static void Run<~0~>(this IVisualTreeRunner vr, System.Action<~0~> act, params object[] parameters) => 
    vr.RunMethod(act, parameters, out var _);")]
    [MacroCode(@"public static TR Run<~0~, TR>(this IVisualTreeRunner vr, System.Func<~0~,TR> act, params object[] parameters) 
    {vr.RunMethod(act, parameters, out var tr); return (TR)tr!;}")]
    public static partial class VisualTreeRunnerOperations
    {
        
    }
}