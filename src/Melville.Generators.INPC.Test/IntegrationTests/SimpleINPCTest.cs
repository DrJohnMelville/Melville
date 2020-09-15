using Melville.TestHelpers.InpcTesting;
using Xunit;
using Melville.Generated;

namespace Melville.Generators.INPC.Test.IntegrationTests
{
    public partial class SimpleINPCTest
    {
        private partial class SimpleObject
        {
            [AutoNotify] private int x;
        }

        [Fact]
        public void TestName()
        {
            var obj = new SimpleObject();
            using var counter = INPCCounter.VerifyInpcFired(obj, i => i.X);
            obj.X = 10;
            Assert.Equal(10, obj.X);
        }

        private partial class HasChangeMethod
        {
            [AutoNotify] private string notifyChanged = "";
            [AutoNotify] private int x;

            partial void WhenXChanges(int oldValue, int newValue)
            {
                NotifyChanged = $"{oldValue} -> {newValue}";
            }
        }
        [Fact]
        public void TestOnChangedMethod()
        {
            var obj = new HasChangeMethod();
            using var counter = INPCCounter.VerifyInpcFired(obj, i=>i.NotifyChanged, i => i.X);
            obj.X = 10;
            Assert.Equal(10, obj.X);
            Assert.Equal("0 -> 10", obj.NotifyChanged);
            
        }
    }
}