using Melville.Generators.INPC.ProductionGenerators.DelegateToGen;
using Melville.Generators.INPC.Test.UnitTests;
using Melville.INPC;
using Xunit;

namespace Melville.Generators.INPC.Test.DelegateToGen;

public class DelegateToSelfTest
{
    private GeneratorTestBed RunTest(string delegateParams,string classMembers) =>
        new(new DelegateToGenerator(), $$"""
            namespace Outer
            {
                using Melville.INPC;
                {{delegateParams}}
                public partial class C
                {
                    {{classMembers}}
                }
            }
            """, typeof(DelegateToAttribute));

    [Fact]
    public void IntToLongTransform()
    {
        var tb =RunTest("[DelegateTo(WrapWith=\"Wrap\", Rename=\"Long$0\")]", "public int X() => 1; private long Wrap(int x)=>x;");
        tb.LastFile().AssertContains("public long LongX() => Wrap(this.X())");
    }
    [Fact]
    public void ForwardPrivateMethods()
    {
        var tb =RunTest("[DelegateTo(WrapWith=\"Wrap\", Rename=\"Long$0\")]", "private int X() => 1; private long Wrap(int x)=>x;");
        tb.LastFile().AssertContains("private long LongX() => Wrap(this.X())");
    }
    [Fact]
    public void ForwardProtectedMethods()
    {
        var tb =RunTest("[DelegateTo(WrapWith=\"Wrap\", Rename=\"Long$0\" Visibility = Visibility.Public)]", "protected int X() => 1; private long Wrap(int x)=>x;");
        tb.LastFile().AssertContains("public long LongX() => Wrap(this.X())");
    }
    [Fact]
    public void DoubleForward()
    {
        var tb =RunTest("[DelegateTo(Filter=\"A.+\" Rename=\"Super$0\")][DelegateTo(Filter=\"B.+\" Rename=\"$0Minor\")]", "private int Apple() => 1; private long Boy(int x)=>x;");
        tb.LastFile().AssertContains("private int SuperApple() => this.Apple();");
        tb.LastFile().AssertContains("private long BoyMinor(int x) => this.Boy(x);");
    }
    [Fact]
    public void ExcludeTest()
    {
        var tb =RunTest("[DelegateTo(Exclude=\"A.+\" Rename=\"Super$0\")][DelegateTo(Exclude=\"B.+\" Rename=\"$0Minor\")]", "private int Apple() => 1; private long Boy(int x)=>x;");
        tb.LastFile().AssertContains("private int AppleMinor() => this.Apple();");
        tb.LastFile().AssertContains("private long SuperBoy(int x) => this.Boy(x);");
        tb.LastFile().AssertDoesNotContain("private int SuperApple() => this.Apple();");
        tb.LastFile().AssertDoesNotContain("private long BoyMinor(int x) => this.Boy(x);");
    }
}