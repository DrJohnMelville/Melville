using Melville.Generators.INPC.ProductionGenerators.DelegateToGen;
using Melville.Generators.INPC.Test.UnitTests;
using Xunit;

namespace Melville.Generators.INPC.Test.DelegateToGen;

public class DelegateToClassTest
{
    private GeneratorTestBed RunTest(string classDecl, string classMembers, string s) => 
        new(new DelegateToGenerator(), $$"""
            namespace Melville.INPC 
            {
               public sealed class DelegateToAttribute : Attribute
                {
                    public DelegateToAttribute(bool explicitImplementation = false){}
                }
            }
            namespace Outer
            {
                using Melville.INPC;
                public {{classDecl}} Delegated
                {
                    {{classMembers}}
                }    
                public partial class C: Delegated 
                {
                    {{s}}}
                }
            }
            """);

    [Fact]
    public void DelegateToClass()
    {
        var res = RunTest("class", "public virtual int Foo()=>1;", "[DelegateTo] Delegated bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("public override int Foo() => this.bar.Foo();");            

    }

    [Fact]
    public void DelegateDefaultEnumValue()
    {
        var res = RunTest("class", "public enum CEnum {Zero, One}; public virtual void Meth(CEnum c = CEnum.One){}",
             "[DelegateTo] Delegated bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("public override void Meth(Outer.Delegated.CEnum c = (Outer.Delegated.CEnum)1) => this.bar.Meth(c);");
    }

    [Fact] public void ForwardOverload()
    {
        var res = RunTest("class", "public virtual int Foo()=>1; public virtual int Foo(int i)=>i;", 
            "[DelegateTo] Delegated bar; public override int Foo(int i)=>1;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("public override int Foo() => this.bar.Foo();");            

    }
    [Fact] public void ForwardInternalOverload()
    {
        var res = RunTest("class", "internal virtual int Foo()=>1; ", 
            "[DelegateTo] Delegated bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("internal override int Foo() => this.bar.Foo();");
    }
    [Fact] public void ForwardProtectedOverload()
    {
        var res = RunTest("class", "protected virtual int Foo()=>1; ", 
            "[DelegateTo] Delegated bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("protected override int Foo() => this.bar.Foo();");
    }
    [Fact] public void ForwardOverloadNumber()
    {
        var res = RunTest("class", "public virtual int Foo(int i, int j)=>1; public virtual int Foo(int i)=>i;", 
            "[DelegateTo] Delegated bar; public override int Foo(int i)=>1;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("public override int Foo(int i, int j) => this.bar.Foo(i, j);");            

    }
    [Fact] public void ForwardOverloadType()
    {
        var res = RunTest("class", "public virtual int Foo(double d)=>1; public virtual int Foo(int i)=>i;", 
            "[DelegateTo] Delegated bar; public override int Foo(int i)=>1;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("public override int Foo(double d) => this.bar.Foo(d);");            

    }
    [Fact]
    public void DoNotForwardExistingMethod()
    {
        var res = RunTest("class", "public virtual int Foo()=>1;", "[DelegateTo] Delegated bar; public override int Foo()=>1;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertDoesNotContain("Foo");
    }
    [Fact]
    public void DelegateAbstractMethod()
    {
        var res = RunTest("abstract class", "public abstract int Foo();", "[DelegateTo] Delegated bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("public override int Foo() => this.bar.Foo();");            

    }
    [Fact]
    public void DelegateProtectedMethod()
    {
        var res = RunTest("class", "protected virtual int Foo()=>1;", "[DelegateTo] Delegated bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("protected override int Foo()");            

    }
    [Fact]
    public void DoNotDelegateNonVirtualMethods()
    {
        var res = RunTest("class", "public int Foo()=>1;", "[DelegateTo] Delegated bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertDoesNotContain("Foo");            

    }
}