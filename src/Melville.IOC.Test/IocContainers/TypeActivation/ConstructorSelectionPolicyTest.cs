using System;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;
using Moq;
using Xunit;

namespace Melville.IOC.Test.IocContainers.TypeActivation
{
    public class ConstructorSelectionPolicyTest
    {
        public class Simple{}

        public class Complex
        {
            public string Data { get; }
            
            public Complex() {Data = "No Arguments";}
            public Complex(Simple a1) {Data = "One Argument";}
            public Complex(Simple a1, int i) {Data = "One Argument + " + i;}
            public Complex(Simple a1, Simple a2) {Data = "Two Arguments";}
            public Complex(Simple a1, Simple a2, Simple a3) {Data = "Three Arguments";}
        }
        
        private readonly IocContainer sut = new IocContainer();

        [Fact]
        public void SingleConstructorRegistration()
        {
            sut.Bind<Simple>().To<Simple>();
        }

        [Fact]
        public void SingleConstructorFailsWithMultipleConstructors()
        {
            Assert.Throws<InvalidOperationException>(() => sut.Bind<Complex>().To<Complex>(i => i.SingleConstructor()));
        }

        [Fact]
        public void MostNumerous()
        {
            var ret = sut.Get<Complex>();
            Assert.Equal("Three Arguments", ret.Data);
        }

        [Fact]
        public void PickDefaultConstructor()
        {
            sut.Bind<Complex>().To<Complex>(i => i.WithArgumentTypes());
            Assert.Equal("No Arguments", sut.Get<Complex>().Data);
        }
        [Fact]
        public void PickAConstructor()
        {
            sut.Bind<Complex>().To<Complex>(i => i.WithArgumentTypes<Simple>());
            Assert.Equal("One Argument", sut.Get<Complex>().Data);
        }
        [Fact]
        public void PickSecondConstructor()
        {
            sut.Bind<Complex>().To<Complex>(i => i.WithArgumentTypes<Simple, Simple>());
            Assert.Equal("Two Arguments", sut.Get<Complex>().Data);
        }

        [Fact]
        public void PickArgumentByType()
        {
            sut.Bind<Complex>().ToSelf(i=>i.WithArgumentTypes<Simple, int>());
            var ret = sut.Get<Func<int, Complex>>()(10);
            Assert.Equal("One Argument + 10", ret.Data);
        }
        
    }
}