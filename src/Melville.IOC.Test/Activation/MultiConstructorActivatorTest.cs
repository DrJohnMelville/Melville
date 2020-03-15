using System;
using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.Activation
{
    public class MultiConstructorActivatorTest
    {
        private readonly IocContainer sut = new IocContainer();
        public class HasMultipleConstructors
        {
            public string StrValue { get; }

            public HasMultipleConstructors() : this(100){}

            public HasMultipleConstructors(int intValue) : this(intValue.ToString()) { }

            public HasMultipleConstructors(string StrValue)
            {
                this.StrValue = StrValue;
            }
        }

        [Fact]
        public void ConstructFromStringFactory()
        {
            var value = sut.Get<Func<string, HasMultipleConstructors>>()("Foo Bar");
            Assert.Equal("Foo Bar", value.StrValue);
        }
        [Fact]
        public void ConstructFromIntFactory()
        {
            var value = sut.Get<Func<int, HasMultipleConstructors>>()(34);
            Assert.Equal("34", value.StrValue);
        }

        [Fact]
        public void DefaultConstructor()
        {
            Assert.Equal("100", sut.Get<HasMultipleConstructors>().StrValue);
        }
    }
}