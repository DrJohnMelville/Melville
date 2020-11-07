using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melville.Generators.INPC.Test.UnitTests;
using Melville.INPC;
using Xunit;

namespace Melville.Generators.INPC.Test.IntegrationTests
{
    public partial class GenericTest
    {
        [AutoNotify] private List<int> numbers = new List<int>();
    
        [Fact]
        public void PropExists()
        {
            Assert.NotNull(Numbers);
        }

        [AutoNotify] private IDictionary<Lazy<OuterClass.InnerClass?>,
            Task<IComparable<CodeWriterTests>>>? complexProp;

        [Fact]
        public void TestName()
        {
            Assert.Null(complexProp);
        }
    }

    public class OuterClass
    {
        public class InnerClass
        {
            
        }
    }
}