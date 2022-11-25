using System.Runtime.CompilerServices.ProductionGenerators.Constructors;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen;
using Melville.Generators.INPC.Test.UnitTests;
using Xunit;

namespace Melville.Generators.INPC.Test.FromConstructorGen;

public class ConstructorGenTest
{
    private GeneratorTestBed RunTest(string s, string parentCode = "") =>
        RunTestWithDeclaredAttr($@"
    public class Parent {{" + parentCode + @"} 
    public partial class C: Parent {" + s + @"
");
    private GeneratorTestBed RunTestWithDeclaredAttr(string s) =>
        new(new ConstructorGenerator(), $$"""
            namespace Melville.INPC
            {
                public class FromConstructorAttribute: System.Attribute{}
            }
            namespace Outer
            {
                using Melville.INPC;
                {{s}}
            }
            """);

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

    [Fact]
    public void RecursiveConstructors()
    {
        var tb = RunTest("[FromConstructor] int b;", "[FromConstructor] int a;");
        tb.LastFileContains("public C(int a, int b): base(a)");
    }
    [Fact]
    public void Recursive2ParamConstructors()
    {
        var tb = RunTest("[FromConstructor] int c;", "[FromConstructor] int a; [FromConstructor] string B {get;}");
        tb.LastFileContains("public C(int a, string b, int c): base(a, b)");
    }
    [Fact]
    public void Recursive2ParamConstructorsReverse()
    {
        var tb = RunTest("[FromConstructor] int c;", "[FromConstructor] string B {get;} [FromConstructor] int a; ");
        tb.LastFileContains("public C(string b, int a, int c): base(b, a)");
    }

    [Fact]
    public void ThreeLevel()
    {
        var tb = RunTestWithDeclaredAttr(@"
                    public class CA { [FromConstructor] float a; public CA(uint y) {}}
                    public class CB: CA { [FromConstructor] double b; public CB(string x) {}}
                    public class CC: CB { [FromConstructor] CA c;}
");
        tb.LastFileContains("public CC(string x, Outer.CA c): base(x)");
        tb.LastFileContains("public CC(uint y, double b, Outer.CA c): base(y, b)");
        tb.LastFileContains("public CC(float a, double b, Outer.CA c): base(a, b)");
    }
    [Fact]
    public void ClassLevelProperty()
    {
        var tb = RunTestWithDeclaredAttr(@"
                    public class CA { [FromConstructor] float a; public CA(uint y) {}}
                    [FromConstructor] public class CB: CA {  }
                    public class CC: CB { [FromConstructor] CA c;}
");
        tb.LastFileContains("public CC(uint y, Outer.CA c): base(y)");
        tb.LastFileContains("public CC(float a, Outer.CA c): base(a)");
    }
    [Fact]
    public void SimpleNoParentCase()
    {
        var tb = RunTestWithDeclaredAttr(@"
                    public class CC { [FromConstructor] int c;}
");
        tb.LastFileContains("public CC(int c)");
    }     
    [Fact]
    public void JustCallParent()
    {
        var tb = RunTestWithDeclaredAttr(@"
                    Public class CA {public CA(int x) {} }
                    [FromConstructor]public class CC:CA { }
");
        tb.LastFileContains("public CC(int x): base(x)");
    }  
}