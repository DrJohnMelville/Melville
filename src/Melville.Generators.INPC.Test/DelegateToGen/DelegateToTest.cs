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
        private GeneratorTestBed RunTest(string s) => new(new DelegateToGenerator(), @"
using Melville.DelegateToGeneration;
namespace Outer
{
    public interface IInterface
    {
        void A();
        void B(int x);
        int C(int x);
        IInterface D(IInterface x, int y, IInterface z);
    }
    public partial class C {" +
                                                   s +
                                                   @"
}
}
");

        private GeneratorTestBed RunTestOnField(string s) =>
            RunTest(s + " private IInterface field;");

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
            var res = RunTest("[DelegateTo] "+member);
            res.FileContains("C.DelegateToGeneration.cs",
                "public partial class C : Outer.IInterface");            
        }

        [Fact]
        public void IplementsAllMethods()
        {
            var res = RunTest(" [DelegateTo] private IInterface Field; ");
            res.FileContains("C.DelegateToGeneration.cs",
                "public void A() => this.field.A();");
        }
    }
}