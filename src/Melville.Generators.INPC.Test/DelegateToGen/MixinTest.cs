using System;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen;
using Melville.Generators.INPC.Test.UnitTests;
using Melville.INPC;
using Xunit;

namespace Melville.Generators.INPC.Test.DelegateToGen;

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
        res.LastFile().AssertContains("public int Foo() => this.bar.Foo();");

    }
    [Fact]
    public void DelegateToInterface()
    {
        var res = RunTest("interface", "public virtual int Foo()=>1;", "[DelegateTo] Mixin bar;");
        res.LastFile().AssertContains("public int Foo() => this.bar.Foo();");
    }
    [Fact]
    public void DelegateToStruct()
    {
        var res = RunTest("struct", "public int Foo()=>1;", "[DelegateTo] Mixin bar;");
        res.LastFile().AssertContains("public int Foo() => this.bar.Foo();");
    }

    [Fact]
    public void DelegateInternaItem()
    {
        var res = RunTest("class", "internal int Foo()=>1;", "[DelegateTo] Mixin bar;");
        res.LastFile().AssertContains("internal int Foo() => this.bar.Foo();");
    }
    [Fact]
    public void SwitchInternalToPublic()
    {
        var res = RunTest("class", "internal int Foo()=>1;", "[DelegateTo(Visibility = SourceLocationVisibility.Public)] Mixin bar;");
        res.LastFile().AssertContains("public int Foo() => this.bar.Foo();");
    }
    [Fact]
    public void FilterByRegex()
    {
        var res = RunTest("class", "internal int Foo()=>1; internal int Baz()=>2", "[DelegateTo(Filter=\"F.*\")] Mixin bar;");
        res.LastFile().AssertContains("internal int Foo() => this.bar.Foo();");
        res.LastFile().AssertDoesNotContain("Baz");
    }
    [Fact]
    public void FilterAndRename()
    {
        var res = RunTest("class", "internal int Foo()=>1; internal int Baz()=>2", "[DelegateTo(Filter=\"(F)(.*)\", Rename=\"$2_$1$1\")] Mixin bar;");
        res.LastFile().AssertContains("internal int oo_FF() => this.bar.Foo();");
        res.LastFile().AssertDoesNotContain("Baz");
    }
}