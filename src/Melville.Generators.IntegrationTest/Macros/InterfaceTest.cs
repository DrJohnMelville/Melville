using Melville.INPC;
using Xunit;

namespace Melville.Generators.IntegrationTest.Macros;

[MacroItem("A")]
[MacroCode("string Get~0~() => \"~0~\";")]
public partial interface ITestInterface
{
}
public partial interface ITestInterface2
{
    [MacroItem("A")]
    [MacroCode("string Get~0~() => \"~0~\";")]
    int B();
}


public class InterfaceMacroTest
{
    private class ConcreteTestInterface : ITestInterface
    {
    }

    [Fact]
    public void CallOnInterfaceAttribute() => Assert.Equal((string?)"A", (string?)(new ConcreteTestInterface() as ITestInterface).GetA());

    private class ConcreteTestInterface2 : ITestInterface2
    {
        public int B() => 2;
    }

    [Fact]
    public void CallItemOnMemberAttribute() => Assert.Equal((string?)"A", (string?)(new ConcreteTestInterface2() as ITestInterface2).GetA());


}