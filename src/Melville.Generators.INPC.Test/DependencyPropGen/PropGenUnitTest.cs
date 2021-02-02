using Melville.Generators.INPC.Macros;
using Melville.Generators.INPC.Test.UnitTests;
using Xunit;

namespace Melville.Generators.INPC.Test.DependencyPropGen
{
    public class PropGenUnitTest
    {
        private GeneratorTestBed RunTest(string s) =>
            new GeneratorTestBed(new MacroGenerator(), @"
using Melville.DependencyPropGen;
namespace Outer
{
    public partial class C {" +
                                                       s +
                                                       @"
    private void Func();
}
}
");

        [Fact]
        public void CanUseProperty()
        {
            RunTest("[GenerateDP(typeof(int),\"Prop\"]");
        }

    }
}