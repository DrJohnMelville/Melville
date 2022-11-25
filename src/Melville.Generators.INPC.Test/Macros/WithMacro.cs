using Melville.INPC;
using Xunit;

namespace Melville.Generators.INPC.Test.Macros;

public partial class WithMacro
{
    [MacroCode("private int Number~1~() => ~0~;")]
    [MacroCode("// Code: ~0~/~1~", Prefix = "private static void FooGenerated() {", Postfix = "}")]
    [MacroItem(1, "One")]
    [MacroItem(2, "Two")]
    [MacroItem(3, "Three")]
    [Fact]
    public void Method()
    {
        FooGenerated();
        Assert.Equal(1, NumberOne());
        Assert.Equal(2, NumberTwo());
        Assert.Equal(3, NumberThree());
          
    }
}