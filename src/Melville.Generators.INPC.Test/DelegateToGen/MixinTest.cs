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
}