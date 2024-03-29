﻿using System;
using System.Security.Cryptography.X509Certificates;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen;
using Melville.Generators.INPC.Test.UnitTests;
using Melville.INPC;
using Xunit;

namespace Melville.Generators.INPC.Test.DelegateToGen;

public class DelegateToTest
{
    private GeneratorTestBed RunTest(string classMembers, string intMembers) =>
        RunTest($$"""
        using Melville.INPC;
        namespace Outer
        {
            public interface IInterface
            {
                {{intMembers}}  
            }
            public interface IChildInterface: IInterface { void ChildMethod(); }
            public partial class C: IInterface { {{classMembers}}
            }
        }
        """);

    private static GeneratorTestBed RunTest(string code) => 
        new(new DelegateToGenerator(), code, typeof(DelegateToAttribute));

    [Fact]
    public void InheritedInterfaceTest()
    {
        var res = RunTest("""
            using Melville.INPC;
            namespace Outer;

            public interface IChild: IExternalNotifyPropertyChanged 
            {
                /// <summary>
                /// This is Child
                /// </summary>
                public int ChildOp();
            }

            public class ConcreteChild: IChild 
            {
                /// <remarks>
                /// Forwarded method through IChild.Inner
                /// </remarks>
                [DelegateTo] private IChild inner;
            }
            """);
        res.LastFile().AssertContains("/// This is Child");
        res.LastFile().AssertContains("/// Forwarded method through");
        res.LastFile().AssertContains("/// Sends a property change notification");
    }

    [Theory]
    [InlineData("private IInterface Field;", "#pragma warning disable CS1734, CS1735")]
    [InlineData("private IInterface Field;", "this.Field.A()")]
    [InlineData("public IInterface Field{get;}", "this.Field.A()")]
    [InlineData("public IInterface Field => null", "this.Field.A()")]
    [InlineData("public IInterface Field{get;set;}", "this.Field.A()")]
    [InlineData("public IInterface Field() => null", "this.Field().A()")]
    public void InheritFromDelegatedInterface(string member, string methodCall)
    {
        var res = RunTest("[DelegateTo] "+member, @"int A();");
        res.LastFile().AssertContains("public partial class C");            
        res.LastFile().AssertContains(methodCall);            
    }

    [Fact]
    public void CopyDocumentation()
    {
        var res = RunTest("[DelegateTo] IInterface field;", """
        /// <summary>
        /// This is the Summary
        /// </summary>
        int A();
        """);
        res.LastFile().AssertContains("/// This is the Summary");
}

    [Theory]
    [InlineData("[DelegateTo] IInterface i; byte Wrap(int i)=>(byte)i;", "public int A() => Wrap(this.i.A());")]
    [InlineData("[DelegateTo] IInterface i; long Wrap(int i)=>i;", "public int A() => this.i.A();")]
    [InlineData("[DelegateTo] IInterface i; long Wrap()=>1;", "public void B() {this.i.B(); Wrap();}")]
    public void ReturnMethodWrapping(string member, string methodCall)
    {
        var res = RunTest("[DelegateTo(WrapWith = \"Wrap\")] " + member, @"int A(); void B();");
        res.LastFile().AssertContains("public partial class C");
        res.LastFile().AssertContains(methodCall);
    }

    [Theory]
    [InlineData(" [DelegateTo] private IInterface Field; ", "get => this.Field.A;")]
    [InlineData(" [DelegateTo] private IInterface Field {get;} ", "get => this.Field.A;")]
    [InlineData(" [DelegateTo] private IInterface Field() ", "get => this.Field().A;")]
    public void DelegationPrefixes(string targetMember, string output)
    {
        var res = RunTest(targetMember, "int A {get;}");
        res.LastFile().AssertContains(output);
    }
    [Theory]
    [InlineData("int A {get;}", "public int A")]
    [InlineData("[System.Obsolete( \"xxyy\")] int A {get;}", "[System.Obsolete(\"xxyy\")] public int A")]
    [InlineData("int A {get;}", "get => this.Field.A;")]
    [InlineData("int A {get;set;}", "get => this.Field.A;")]
    [InlineData("int A {get;set;}", "set => this.Field.A = value;")]
    [InlineData("int A {set;}", "set => this.Field.A = value;")]
    [InlineData("int A {get;init;}", "init => this.Field.A = value;")]
    [InlineData("int A();", "public int A() => this.Field.A();")]
    [InlineData("internal int A();", "internal int A() => this.Field.A();")]
    [InlineData("int A(ref int b);", "public int A(ref int b) => this.Field.A(ref b);")]
    [InlineData("int A(out int b);", "public int A(out int b) => this.Field.A(out b);")]
    [InlineData("int A(in int b);", "public int A(in int b) => this.Field.A(in b);")]
    [InlineData("int A(int a);", "public int A(int a) => this.Field.A(a);")]
    [InlineData("int A(int a = 1);", "public int A(int a = 1) => this.Field.A(a);")]
    [InlineData("int A(string? a = null);", "public int A(string? a = default) => this.Field.A(a);")]
    [InlineData("int A([System.Obsolete(\"xxx\")] out string? a);", "public int A([System.Obsolete(\"xxx\")] out string? a) => this.Field.A(out a);")]
    [InlineData("int A(bool a = true);", "public int A(bool a = true) => this.Field.A(a);")]
    [InlineData("int A(bool a = false);", "public int A(bool a = false) => this.Field.A(a);")]
    [InlineData("int A(int a, string b);", "public int A(int a, string b) => this.Field.A(a, b);")]
    [InlineData("T A<T>();", "public T A<T>() => this.Field.A<T>();")]
    [InlineData("T A<T,T2>();", "public T A<T,T2>() => this.Field.A<T,T2>();")]
    [InlineData("IList<T> A<T>(T a);", "public IList<T> A<T>(T a) => this.Field.A<T>(a);")]
    [InlineData("void A();", "public void A() => this.Field.A();")]
    [InlineData("event EventHandler A;", "public event EventHandler A")]
    [InlineData("event EventHandler A;", "add => this.Field.A += value;")]
    [InlineData("event EventHandler A;", "remove => this.Field.A -= value;")]
    [InlineData("int this[int a] {get;}", "public int this[int a]")]
    [InlineData("int this[int a] {get;}", "get => this.Field[a];")]
    [InlineData("int this[int a] {get; set;}", "get => this.Field[a];")]
    [InlineData("int this[int a] {get; set;}", "set => this.Field[a] = value;")]
    public void ImplementsMembers(string intMember, string output)
    {
        var res = RunTest(" [DelegateTo] private IInterface Field; ", intMember);
        res.LastFile().AssertContainsIgnoreWhiteSpace(output);
    }
    
    [Theory]
    [InlineData("int A(ref int b);", "int Outer.IInterface.A(ref int b) => this.Field.A(ref b);")]
    [InlineData("int A(out int b);", "int Outer.IInterface.A(out int b) => this.Field.A(out b);")]
    [InlineData("int A(in int b);", "int Outer.IInterface.A(in int b) => this.Field.A(in b);")]
    [InlineData("int A(int a);", "int Outer.IInterface.A(int a) => this.Field.A(a);")]
    [InlineData("int A(int a = 1);", "int Outer.IInterface.A(int a = 1) => this.Field.A(a);")]
    [InlineData("int A(string? a = null);", "int Outer.IInterface.A(string? a = default) => this.Field.A(a);")]
    [InlineData("int A([System.Obsolete(\"xxx\")] out string? a);", "int Outer.IInterface.A([System.Obsolete(\"xxx\")] out string? a) => this.Field.A(out a);")]
    [InlineData("int A(bool a = true);", "int Outer.IInterface.A(bool a = true) => this.Field.A(a);")]
    [InlineData("int A(bool a = false);", "int Outer.IInterface.A(bool a = false) => this.Field.A(a);")]
    [InlineData("int A(int a, string b);", "int Outer.IInterface.A(int a, string b) => this.Field.A(a, b);")]
    [InlineData("T A<T>();", "T Outer.IInterface.A<T>() => this.Field.A<T>();")]
    [InlineData("T A<T,T2>();", "T Outer.IInterface.A<T,T2>() => this.Field.A<T,T2>();")]
    [InlineData("IList<T> A<T>(T a);", "IList<T> Outer.IInterface.A<T>(T a) => this.Field.A<T>(a);")]
    [InlineData("void A();", "void Outer.IInterface.A() => this.Field.A();")]
    public void ExplicitImplementation(string intMember, string output)
    {
        var res = RunTest(" [DelegateTo(true)] private IInterface Field; ", intMember);
        res.LastFile().AssertContains(output);
    }

    [Theory]
    [InlineData("int A(ref int b);", "int Outer.IInterface.A(ref int b) => this.Field.A(ref b);")]
    public void ExplicitImplementationAsProperty(string intMember, string output)
    {
        var res = RunTest(" [DelegateTo(ExplicitImplementation = true)] private IInterface Field; ", intMember);
        res.LastFile().AssertContains(output);
    }


    [Fact]
    public void DoNotGenerateExistingMembers()
    {
        var res = RunTest(" [DelegateTo] private IInterface Field; public int A()=>1;",
            "int A(); int B(int b1);");
        res.LastFile().AssertContains("int B(int b1)");
        res.LastFile().AssertDoesNotContain("int A(");
    }

    [Theory]
    [InlineData("public int A();", "public int A(){}", "public int A1() {}")]
    [InlineData("public int A();", "public int A(){}", "public int A(string s) {}")]
    [InlineData("public int A(int x);", "public int A(int y){}", "public int A(string s) {}")]
    [InlineData("public int A(int x);", "public int A(int y){}", "public int A() {}")]
    [InlineData("public int A<T>();", "public int A<T>(){}", "public int A() {}")]
    [InlineData("public int A<T>();", "public int A<T>(){}", "public int A<T1,T2>() {}")]
    public void MethodSupressionTest(string hostMember, string suppressed, string notSupressed)
    {
        RunTest("[DelegateTo] private IInterface field; " + hostMember, suppressed).LastFile().AssertDoesNotContain("public int");
        RunTest("[DelegateTo] private IInterface field; " + hostMember, notSupressed).LastFile().AssertContains("public int");

    }
        
    [Theory]
    [InlineData("int A {get;set;}", "get_A")]
    [InlineData("int A {get;set;}", "set_A")]
    public void DoNotImplementHiddenMethods(string intMember, string output)
    {
        var res = RunTest(" [DelegateTo] private IInterface Field; ", intMember);
        res.LastFile().AssertDoesNotContain(output);
    }
        
    [Fact]
    public void InheritedInterface()
    {
        var res = RunTest("[DelegateTo] private IChildInterface Field;", "int Parent();");
        res.LastFile().AssertContains("public void ChildMethod() => this.Field.ChildMethod();");
        res.LastFile().AssertContains("public int Parent() => this.Field.Parent();");
    }

    [Fact]
    public void PropogatePropertyToMethod()
    {
        var res = RunTest("[DelegateTo] [method: MethAttr][property: PropAttr] private IChildInterface Field;", "int ChildProp{get;}");
        res.LastFile().AssertContainsIgnoreWhiteSpace(@"[MethAttr] public void ChildMethod");
        res.LastFile().AssertContainsIgnoreWhiteSpace(@"[PropAttr] public int ChildProp {");
    }
}