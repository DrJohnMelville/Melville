using System.Runtime.CompilerServices.ProductionGenerators.Constructors;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen;
using Melville.Generators.INPC.Test.UnitTests;
using Xunit;

namespace Melville.Generators.INPC.Test.FromConstructorGen;

public class ConstructorGenTest
{
    private GeneratorTestBed RunTest(string s, string parentCode = "") =>
        new(new ConstructorGenerator(), @"
using Melville.INPC;
namespace Outer
{
    public class Parent {"+parentCode+@"} 
    public partial class C: Parent {" + s + @"
}
}");

    [Fact]
    public void IntProperty()
    {
        var tb = RunTest("[FromConstructor] private int I {get;}");
        tb.LastFileContains("public partial class C");
        tb.LastFileContains("public C(int i)");
        tb.LastFileContains("this.I = i;");
    }
    [Fact]
    public void PartialOnConstructedMethod()
    {
        var tb = RunTest("[FromConstructor] private int I {get;}");
        tb.LastFileContains("public partial class C");
        tb.LastFileContains("OnConstructed();");
        tb.LastFileContains("partial void OnConstructed();");
    }
    [Fact]
    public void AvoidKeywords()
    {
        var tb = RunTest("[FromConstructor] private int If {get;}");
        tb.LastFileContains("public partial class C");
        tb.LastFileContains("public C(int @if)");
        tb.LastFileContains("If = @if;");
    }
    [Fact]
    public void Fields()
    {
        var tb = RunTest("[FromConstructor] private readonly int i;");
        tb.LastFileContains("public partial class C");
        tb.LastFileContains("public C(int i)");
        tb.LastFileContains("this.i = i;");
    }
    [Fact]
    public void MultipleFieldsInOneDeclaration()
    {
        var tb = RunTest("[FromConstructor] private readonly int i,j;");
        tb.LastFileContains("public partial class C");
        tb.LastFileContains("public C(int i, int j)");
        tb.LastFileContains("this.i = i;");
        tb.LastFileContains("this.j = j;");
    }
    [Fact]
    public void MultipleFieldsInTwoDeclarations()
    {
        var tb = RunTest("[FromConstructor] private readonly int i;" +
                         "[FromConstructor] public double j;");
        tb.LastFileContains("public partial class C");
        tb.LastFileContains("public C(int i, double j)");
        tb.LastFileContains("this.i = i;");
        tb.LastFileContains("this.j = j;");
    }

    [Fact]
    public void InheritedField()
    {
        var tb = RunTest("[FromConstructor] int j;", "public Parent(uint i) {}");
        tb.LastFileContains("public C(uint i, int j): base(i)");
        tb.LastFileDoesNotContain("this.i");
        tb.LastFileContains("this.j = j;");
    }
    [Fact]
    public void TwoInheritedConstructors()
    {
        var tb = RunTest("[FromConstructor] int j;", @"
                   public Parent(uint i) {} 
                   public Parent(uint i, string k) {}");
        tb.LastFileContains("public C(uint i, int j): base(i)");
        tb.LastFileContains("public C(uint i, string k, int j): base(i, k)");
        tb.LastFileDoesNotContain("this.i");
        tb.LastFileDoesNotContain("this.k");
        tb.LastFileContains("this.j = j;");
    }
}