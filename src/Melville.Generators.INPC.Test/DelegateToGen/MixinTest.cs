using System;
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
    public void DoubleForward()
    {
        var tb =RunTest("[DelegateTo(Filter=\"A.+\" Rename=\"Super$0\")][DelegateTo(Filter=\"B.+\" Rename=\"$0Minor\")]", "private int Apple() => 1; private long Boy(int x)=>x;");
        tb.LastFile().AssertContains("private int SuperApple() => this.Apple();");
        tb.LastFile().AssertContains("private long BoyMinor(int x) => this.Boy(x);");
    }
}

public class MixinTest
{
    private GeneratorTestBed RunTest(string classDecl, string classMembers, string s) =>
        new(new DelegateToGenerator(), $$"""
            namespace Outer
            {
                using Melville.INPC;
                public {{classDecl}} Mixin
                {
                    {{classMembers}}
                }     
                public partial class C
                {
                    {{s}}}
                }
            }
            """, typeof(DelegateToAttribute));

    [Fact]
    public void DelegateToClass()
    {
        var res = RunTest("class", "public virtual int Foo()=>1;", "[DelegateTo] Mixin bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("public int Foo() => this.bar.Foo();");

    }
    [Fact]
    public void DelegateToInterface()
    {
        var res = RunTest("interface", "public virtual int Foo()=>1;", "[DelegateTo] Mixin bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("public int Foo() => this.bar.Foo();");
    }
    [Fact]
    public void DelegateToStruct()
    {
        var res = RunTest("struct", "public int Foo()=>1;", "[DelegateTo] Mixin bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("public int Foo() => this.bar.Foo();");
    }

    [Fact]
    public void DelegateInternaItem()
    {
        var res = RunTest("class", "internal int Foo()=>1;", "[DelegateTo] Mixin bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("internal int Foo() => this.bar.Foo();");
    }
    [Fact]
    public void SwitchInternalToPublic()
    {
        var res = RunTest("class", "internal int Foo()=>1;", "[DelegateTo(Visibility = Visibility.Public)] Mixin bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("public int Foo() => this.bar.Foo();");
    }
    [Fact]
    public void FilterByRegex()
    {
        var res = RunTest("class", "internal int Foo()=>1; internal int Baz()=>2", "[DelegateTo(Filter=\"F.*\")] Mixin bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("internal int Foo() => this.bar.Foo();");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertDoesNotContain("Baz");
    }
    [Fact]
    public void FilterAndRename()
    {
        var res = RunTest("class", "internal int Foo()=>1; internal int Baz()=>2", "[DelegateTo(Filter=\"(F)(.*)\", Rename=\"$2_$1$1\")] Mixin bar;");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertContains("internal int oo_FF() => this.bar.Foo();");
        res.FromName("GeneratedDelegator.Outer.C.bar.cs").AssertDoesNotContain("Baz");
    }
}