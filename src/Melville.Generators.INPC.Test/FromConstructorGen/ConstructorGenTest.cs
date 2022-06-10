using System.Runtime.CompilerServices.ProductionGenerators.Constructors;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen;
using Melville.Generators.INPC.Test.UnitTests;
using Xunit;

namespace Melville.Generators.INPC.Test.FromConstructorGen;

public class ConstructorGenTest
{
    private GeneratorTestBed RunTest(string s) =>
        new(new ConstructorGenerator(), @"
using Melville.INPC;
namespace Outer
{
    public partial class C {" + s + @"
}
}");

    [Fact]
    public void IntProperty()
    {
        var tb = RunTest("[FromConstructor] private readonly int I {get;}");
        tb.LastFileContains("public partial class C");
        tb.LastFileContains("public C(int i)");
        tb.LastFileContains("this.I = i;");
    }
    [Fact]
    public void AvoidKeywords()
    {
        var tb = RunTest("[FromConstructor] private readonly int If {get;}");
        tb.LastFileContains("public partial class C");
        tb.LastFileContains("public C(int @if)");
        tb.LastFileContains("If = @if;");
    }
}