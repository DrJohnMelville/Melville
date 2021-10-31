using System;
using Melville.Generators.INPC.DelegateToGen;
using Melville.Generators.INPC.Test.UnitTests;
using Xunit;

namespace Melville.Generators.INPC.Test.DelegateToGen;

public class DelegateToTest
{
    private GeneratorTestBed RunTest(string s, string intMembers) => new(new DelegateToGenerator(), @"
using Melville.INPC;
namespace Outer
{
    public interface IInterface
    {"+intMembers+@"}
    public interface IChildInterface: IInterface { void ChildMethod(); }
    public partial class C: IInterface {" + s + @"
}
}");
    [Theory]
    [InlineData("private IInterface Field;", "this.Field.A()")]
    [InlineData("public IInterface RProp{get;}", "this.RProp.A()")]
    [InlineData("public IInterface RProp => null", "this.RProp.A()")]
    [InlineData("public IInterface Prop{get;set;}", "this.Prop.A()")]
    [InlineData("public IInterface Method() => null", "this.Method().A()")]
    public void InheritFromDelegatedInterface(string member, string methodCall)
    {
        var res = RunTest("[DelegateTo] "+member, @"int A();");
        res.FileContains("C.DelegateToGeneration.cs", "public partial class C");            
        res.FileContains("C.DelegateToGeneration.cs", methodCall);            
    }

    [Theory]
    [InlineData(" [DelegateTo] private IInterface Field; ", "get => this.Field.A;")]
    [InlineData(" [DelegateTo] private IInterface Field {get;} ", "get => this.Field.A;")]
    [InlineData(" [DelegateTo] private IInterface Field() ", "get => this.Field().A;")]
    public void DelegationPrefixes(string targetMember, string output)
    {
        var res = RunTest(targetMember, "int A {get;}");
        res.FileContains("C.DelegateToGeneration.cs",
            output);
    }
    [Theory]
    [InlineData("int A {get;}", "public int A")]
    [InlineData("[System.Obsolete( \"xxyy\")] int A {get;}", "[System.ObsoleteAttribute(\"xxyy\")] public int A")]
    [InlineData("[System.Obsolete( \"xxyy\")][System.Runtime.CompilerServices.NullableContext(2] int A {get;}", "[System.ObsoleteAttribute(\"xxyy\")] public int A")]
    [InlineData("int A {get;}", "get => this.Field.A;")]
    [InlineData("int A {get;set;}", "get => this.Field.A;")]
    [InlineData("int A {get;set;}", "set => this.Field.A = value;")]
    [InlineData("int A {set;}", "set => this.Field.A = value;")]
    [InlineData("int A {get;init;}", "init => this.Field.A = value;")]
    [InlineData("int A();", "public int A() => this.Field.A();")]
    [InlineData("int A(ref int b);", "public int A(ref int b) => this.Field.A(ref b);")]
    [InlineData("int A(out int b);", "public int A(out int b) => this.Field.A(out b);")]
    [InlineData("int A(in int b);", "public int A(in int b) => this.Field.A(in b);")]
    [InlineData("int A(int a);", "public int A(int a) => this.Field.A(a);")]
    [InlineData("int A(int a = 1);", "public int A(int a = 1) => this.Field.A(a);")]
    [InlineData("int A(string? a = null);", "public int A(string? a = default) => this.Field.A(a);")]
    [InlineData("int A([System.Obsolete(\"xxx\")] out string? a);", "public int A([System.ObsoleteAttribute(\"xxx\")] out string? a) => this.Field.A(out a);")]
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
        res.FileContains("C.DelegateToGeneration.cs",
            output);
    }

    [Fact]
    public void DoNotGenerateExistingMembers()
    {
        var res = RunTest(" [DelegateTo] private IInterface Field; public int A()=>1;",
            "int A(); int B(int b1);");
        res.FileContains("C.DelegateToGeneration.cs", "int B(int b1)");
        res.FileDoesNotContain("C.DelegateToGeneration.cs", "int A(");
    }
        
    [Theory]
    [InlineData("int A {get;set;}", "get_A")]
    [InlineData("int A {get;set;}", "set_A")]
    public void DoNotImplementHiddenMethods(string intMember, string output)
    {
        var res = RunTest(" [DelegateTo] private IInterface Field; ", intMember);
        res.FileDoesNotContain("C.DelegateToGeneration.cs",
            output);
    }
        
    [Fact]
    public void InheritedInterface()
    {
        var res = RunTest("[DelegateTo] private IChildInterface field;", "int Parent();");
        res.FileContains("C.DelegateToGeneration.cs", "public void ChildMethod() => this.field.ChildMethod();");
        res.FileContains("C.DelegateToGeneration.cs", "public int Parent() => this.field.Parent();");
    }
}