using Melville.Generators.INPC.DelegateToGen;
using Melville.Generators.INPC.Test.UnitTests;
using Xunit;

namespace Melville.Generators.INPC.Test.DelegateToGen
{
    // public interface IInterface
    // {
    //     void A();
    //     void B(int x);
    //     int C(int x);
    //     IInterface D(IInterface x, int y, IInterface z);
    // }
    public class DelegateToTest
    {
        private GeneratorTestBed RunTest(string s, string intMembers) => new(new DelegateToGenerator(), @"
using Melville.DelegateToGeneration;
namespace Outer
{
    public interface IInterface
    {"+intMembers+@"}
    public partial class C {" +
                                                                                                        s +
                                                                                                        @"
}
}
");

        private GeneratorTestBed RunTestOnField(string s) =>
            RunTest(s + " private IInterface field;", "");

        [Fact]
        public void GenerateProperty()
        {
            var res = RunTestOnField("[DelegateTo]");
            res.FileContains("DelegateToGenerationAttributes.DelegateToGeneration.cs",
                "internal sealed class DelegateToAttribute: Attribute");
            res.FileContains("DelegateToGenerationAttributes.DelegateToGeneration.cs",
                " [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, Inherited=false)]");
            res.FileContains("DelegateToGenerationAttributes.DelegateToGeneration.cs",
                "public DelegateToAttribute(bool explicitImplementation = false){}");
        }

        [Theory]
        [InlineData("private IInterface Field;")]
        [InlineData("public IInterface RProp{get;}")]
        [InlineData("public IInterface RProp => null")]
        [InlineData("public IInterface Prop{get;set;}")]
        [InlineData("public IInterface Method() => null")]
        public void InheritFromDelegatedInterface(string member)
        {
            var res = RunTest("[DelegateTo] "+member, @"");
            res.FileContains("C.DelegateToGeneration.cs",
                "public partial class C : Outer.IInterface");            
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
        [InlineData("int A {get;}", "get => this.Field.A;")]
        [InlineData("int A {get;set;}", "get => this.Field.A;")]
        [InlineData("int A {get;set;}", "set => this.Field.A = value;")]
        [InlineData("int A {set;}", "set => this.Field.A = value;")]
        [InlineData("int A {get;init;}", "init => this.Field.A = value;")]
        [InlineData("int A();", "public int A() => this.Field.A();")]
        [InlineData("int A(int a);", "public int A(int a) => this.Field.A(a);")]
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
        [Theory]
        [InlineData("int A {get;set;}", "get_A")]
        [InlineData("int A {get;set;}", "set_A")]
        public void DoNotImplementHiddenMethods(string intMember, string output)
        {
            var res = RunTest(" [DelegateTo] private IInterface Field; ", intMember);
            res.FileDoesNotContain("C.DelegateToGeneration.cs",
                output);
        }
    }
}