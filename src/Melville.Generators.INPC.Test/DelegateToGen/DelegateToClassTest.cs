using Melville.Generators.INPC.DelegateToGen;
using Melville.Generators.INPC.Test.UnitTests;
using Xunit;

namespace Melville.Generators.INPC.Test.DelegateToGen
{
    public class DelegateToClassTest
    {
        private GeneratorTestBed RunTest(string classDecl, string classMembers, string s) => 
            new(new DelegateToGenerator(), $@"
using Melville.INPC;
namespace Outer
{{
    public {classDecl} Delegated
    {{"+classMembers+@"}
    public partial class C: Delegated {" + s + @"
}
}");

        [Fact]
        public void DelegateToClass()
        {
            var res = RunTest("class", "public virtual int Foo()=>1;", "[DelegateTo] Delegated bar;");
            res.FileContains("C.DelegateToGeneration.cs", "public override int Foo()=>bar.Foo()");            

        }
    }
}