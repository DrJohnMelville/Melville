using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melville.INPC;
using Xunit;

namespace Melville.Generators.IntegrationTest.FromConstructorTest
{
    public class Base
    {
        public Base(int i)
        {
        }
    }

    public partial class Intermed : Base
    {
        [FromConstructor] private string s;
    }

    [FromConstructor]
    public partial class Leaf: Intermed {
    }

    public class FromConstructors
    {
        [Fact]
        public void TryConstruct() => Assert.NotNull(new Leaf(1, "One"));
    }
}
