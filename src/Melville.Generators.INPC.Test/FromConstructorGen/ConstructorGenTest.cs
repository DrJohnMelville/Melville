using Melville.Generators.INPC.ProductionGenerators.Constructors;
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
        tb.LastFile().AssertContains("public partial class C");
        tb.LastFile().AssertContains("public C(int i)");
        tb.LastFile().AssertContains("this.I = i;");
    }
    [Fact]
    public void PartialOnConstructedMethod()
    {
        var tb = RunTest("[FromConstructor] private int I {get;}");
        tb.LastFile().AssertContains("public partial class C");
        tb.LastFile().AssertContains("OnConstructed();");
        tb.LastFile().AssertContains("partial void OnConstructed();");
    }
    [Fact]
    public void AvoidKeywords()
    {
        var tb = RunTest("[FromConstructor] private int If {get;}");
        tb.LastFile().AssertContains("public partial class C");
        tb.LastFile().AssertContains("public C(int @if)");
        tb.LastFile().AssertContains("If = @if;");
    }
   [Fact]
    public void Fields()
    {
        var tb = RunTest("[FromConstructor] private readonly int i;");
        tb.LastFile().AssertContains("public partial class C");
        tb.LastFile().AssertContains("public C(int i)");
        tb.LastFile().AssertContains("this.i = i;");
    }
    [Fact]
    public void MultipleFieldsInOneDeclaration()
    {
        var tb = RunTest("[FromConstructor] private readonly int i,j;");
        tb.LastFile().AssertContains("public partial class C");
        tb.LastFile().AssertContains("public C(int i, int j)");
        tb.LastFile().AssertContains("this.i = i;");
        tb.LastFile().AssertContains("this.j = j;");
    }
    [Fact]
    public void MultipleFieldsInTwoDeclarations()
    {
        var tb = RunTest("[FromConstructor] private readonly int i;" +
                         "[FromConstructor] public double j;");
        tb.LastFile().AssertContains("public partial class C");
        tb.LastFile().AssertContains("public C(int i, double j)");
        tb.LastFile().AssertContains("this.i = i;");
        tb.LastFile().AssertContains("this.j = j;");
    }

    [Fact]
    public void InheritedField()
    {
        var tb = RunTest("[FromConstructor] int j;", "public Parent(uint i) {}");
        tb.LastFile().AssertContains("public C(uint i, int j): base(i)");
        tb.LastFile().AssertDoesNotContain("this.i");
        tb.LastFile().AssertContains("this.j = j;");
    }
    [Fact]
    public void TwoInheritedConstructors()
    {
        var tb = RunTest("[FromConstructor] int j;", @"
                   public Parent(uint i) {} 
                   public Parent(uint i, string k) {}");
        tb.LastFile().AssertContains("public C(uint i, int j): base(i)");
        tb.LastFile().AssertContains("public C(uint i, string k, int j): base(i, k)");
        tb.LastFile().AssertDoesNotContain("this.i");
        tb.LastFile().AssertDoesNotContain("this.k");
        tb.LastFile().AssertContains("this.j = j;");
    }

    [Fact]
    public void RecursiveConstructors()
    {
        var tb = RunTest("[FromConstructor] int b;", "[FromConstructor] int a;");
        tb.LastFile().AssertContains("public C(int a, int b): base(a)");
    }
    [Fact]
    public void Recursive2ParamConstructors()
    {
        var tb = RunTest("[FromConstructor] int c;", "[FromConstructor] int a; [FromConstructor] string B {get;}");
        tb.LastFile().AssertContains("public C(int a, string b, int c): base(a, b)");
    }
    [Fact]
    public void Recursive2ParamConstructorsReverse()
    {
        var tb = RunTest("[FromConstructor] int c;", "[FromConstructor] string B {get;} [FromConstructor] int a; ");
        tb.LastFile().AssertContains("public C(string b, int a, int c): base(b, a)");
    }

    [Fact]
    public void ThreeLevel()
    {
        var tb = RunTestWithDeclaredAttr(@"
                    public class CA { [FromConstructor] float a; public CA(uint y) {}}
                    public class CB: CA { [FromConstructor] double b; public CB(string x) {}}
                    public class CC: CB { [FromConstructor] CA c;}
");
        tb.LastFile().AssertContains("public CC(string x, Outer.CA c): base(x)");
        tb.LastFile().AssertContains("public CC(uint y, double b, Outer.CA c): base(y, b)");
        tb.LastFile().AssertContains("public CC(float a, double b, Outer.CA c): base(a, b)");
    }
    [Fact]
    public void ClassLevelProperty()
    {
        var tb = RunTestWithDeclaredAttr(@"
                    public class CA { [FromConstructor] float a; public CA(uint y) {}}
                    [FromConstructor] public class CB: CA {  }
                    public class CC: CB { [FromConstructor] CA c;}
");
        tb.LastFile().AssertContains("public CC(uint y, Outer.CA c): base(y)");
        tb.LastFile().AssertContains("public CC(float a, Outer.CA c): base(a)");
    }
    [Fact]
    public void ExcludeEmptyConstructor()
    {
        var tb = RunTestWithDeclaredAttr("""
            public class Base
            {
                public Base(int i)
                {
                }
            }

            public partial class Intermed : Base
            {
                [FromConstructor] private string s;
            }

            [FromConstructor]
            public partial class Leaf: Intermed {
            }

            """);
        tb.FromName("Intermed").AssertContains("public Intermed(int i, string s): base(i)");
        tb.FromName("Intermed").AssertDoesNotContain("public Intermed()");
        tb.FromName("Leaf").AssertContains("public Leaf(int i, string s): base(i, s)");
        tb.FromName("Leaf").AssertDoesNotContain("public Leaf()");
    }
    [Fact]
    public void SimpleNoParentCase()
    {
        var tb = RunTestWithDeclaredAttr(@"
                    public class CC { [FromConstructor] int c;}
");
        tb.LastFile().AssertContains("public CC(int c)");
    }     
    [Fact]
    public void JustCallParent()
    {
        var tb = RunTestWithDeclaredAttr(@"
                    Public class CA {public CA(int x) {} }
                    [FromConstructor]public class CC:CA { }
");
        tb.LastFile().AssertContains("public CC(int x): base(x)");
    }  
}