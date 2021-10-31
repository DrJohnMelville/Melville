using Melville.INPC;

namespace Melville.MVVM.Wpf.Bindings;

public static partial class LambdaConverter
{
    [MacroItem("T1, R")]
    [MacroItem("T1, T2, R")]
    [MacroItem("T1, T2, T3, R")]
    [MacroItem("T1, T2, T3, T4, R")]
    [MacroItem("T1, T2, T3, T4, T5, R")]
    [MacroCode(@"public static Melville.MVVM.Wpf.Bindings.MultiConverter Create<~0~>(
        System.Func<~0~> func)=>new(func); ")]
    [MacroCode(@"public static Melville.MVVM.Wpf.Bindings.MultiConverter Create<~0~>(
        System.Func<~0~> func, System.Func<R, object[]> inverse)=>new(func, inverse); ")]
    public static MultiConverter Create<T1, R>(System.Func<T1, R> func, System.Func<R, T1> inverse) => 
        new(func, inverse);
}