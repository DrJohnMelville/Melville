using System;
using Melville.IOC.Activation;
using Xunit;

namespace Melville.IOC.Test.Activation
{
    public class FuncFactoryTest
    {
        public class ObjectTarget
        {
            private object[] Destination { get; set; }

            public object SetDestination(object[] values)
            {
                Destination = values;

                return "Return Value:" + values[0].ToString();
            }
        }

        [Fact]
        public void CompileFactory()
        {
            var target = new ObjectTarget();
            var fact = new ForwardFuncToMethodCall(typeof(Func<string,string>), nameof(ObjectTarget.SetDestination),
                typeof(ObjectTarget));

            var f = (Func<string, string>) fact.CreateFuncDelegate(target);

            Assert.Equal("Return Value:Hello World", f("Hello World"));
        }
        [Fact]
        public void CompileIntFactory()
        {
            var target = new ObjectTarget();
            var fact = new ForwardFuncToMethodCall(typeof(Func<int,string>), nameof(ObjectTarget.SetDestination),
                typeof(ObjectTarget));

            var f = (Func<int, string>) fact.CreateFuncDelegate(target);

            Assert.Equal("Return Value:101", f(101));
        }
    }
}