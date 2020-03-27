using System;
using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.IocContainers
{
    public class SelectTypeByParameterTest
    {
        private interface IBO{};
        private class BO1: IBO{}
        private class BO11:BO1 {}
        private class BO2: IBO{}
        
        private readonly IocContainer sut = new IocContainer();

        [Fact]
        public void ResolveByStringValue()
        {
            sut.Bind<IBO>().To<BO1>().WhenParameterHasValue("1");
            sut.Bind<IBO>().To<BO2>().WhenParameterHasValue("2");
            
            Assert.False(sut.CanGet<IBO>());
            var factory = sut.Get<Func<string, IBO>>();
            Assert.True(factory("1") is BO1);
            Assert.True(factory("2") is BO2);
        }
        
        private interface IVM { IBO Item { get; } }
        private class VM1:IVM
        {
            public VM1(BO1 item)
            {
                Item = item;
            }

            public IBO Item { get; }
        }
        private class VM2:IVM
        {
            public VM2(BO2 item)
            {
                Item = item;
            }

            public IBO Item { get; }
        }

        [Fact]
        public void ResolveViewModelWrappers()
        {
            sut.Bind<IVM>().To<VM1>().WhenParameterHasType<BO1>();
            sut.Bind<IVM>().To<VM2>().WhenParameterHasType<BO2>();

            var func = sut.Get<Func<IBO, IVM>>();
            
            Assert.True(func(new BO1()) is VM1);
            Assert.True(func(new BO11()) is VM1);
            Assert.True(func(new BO2()) is VM2);
        }
    }
}