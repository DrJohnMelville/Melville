using System;
using System.Collections.Generic;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;
using Xunit;

namespace Melville.IOC.Test.IocContainers
{
    public interface ISimpleObject
    {
    }

    public interface ISimpleObject2
    {
    }

    public class SimpleObjectImplementation : ISimpleObject, ISimpleObject2
    {
    }
    public class SecondaryObject
    {
        public ISimpleObject SimpleObject { get; }

        public SecondaryObject(ISimpleObject simpleObject)
        {
            SimpleObject = simpleObject;
        }
    }

    public class IocContainerTest
    {
        private readonly IocContainer sut = new IocContainer();
        
        [Fact]
        public void CreateSimpleObject()
        {
            sut.Bind<SimpleObjectImplementation>().To<SimpleObjectImplementation>();

            Assert.NotNull(sut.Get<SimpleObjectImplementation>());
        }

        [Fact]
        public void BindToInterface()
        {
            sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
            Assert.True(sut.Get<ISimpleObject>() is SimpleObjectImplementation);
        }

        [Fact]
        public void BindMultipleInterfaces()
        {
            sut.Bind<ISimpleObject>()
                .And<SimpleObjectImplementation>()
                .And<ISimpleObject2>()
                .To<SimpleObjectImplementation>();
            Assert.True(sut.Get<ISimpleObject>() is SimpleObjectImplementation);
            Assert.True(sut.Get<SimpleObjectImplementation>() is SimpleObjectImplementation);
            Assert.True(sut.Get<ISimpleObject2>() is SimpleObjectImplementation);
        }

        [Fact]
        public void BindToConstant()
        {
            var single = new SimpleObjectImplementation();
            sut.Bind<ISimpleObject>().ToConstant(single);
            Assert.Equal(single, sut.Get<ISimpleObject>());
        }
        
        [Fact]
        public void Indirection()
        {
            sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
            sut.Bind<SecondaryObject>().To<SecondaryObject>();
            var ret = sut.Get<SecondaryObject>();
            Assert.NotNull(ret);
            Assert.NotNull(ret.SimpleObject);
        }

        [Fact]
        public void BindUsingMethodWithIoc()
        {
            sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
            sut.Bind<SecondaryObject>().ToMethod(i => new SecondaryObject(i.Get<ISimpleObject>()));
            var ret = sut.Get<SecondaryObject>();
            Assert.NotNull(ret.SimpleObject);
        }

        [Fact]
        public void CannotBindOpenGeneric()
        {
            Assert.Throws<IocException>(()=>sut.Get(typeof(IEnumerable<>)));
        }
        [Fact]
        public void BindingFailureMessage()
        {
            try
            {
                sut.Get<SecondaryObject>();
            }
            catch (Exception e)
            {
                Assert.Equal("Cannot bind type: ISimpleObject\r\n    1. Melville.IOC.Test.IocContainers.ISimpleObject -- No Scope\r\n    2. Melville.IOC.Test.IocContainers.SecondaryObject -- No Scope", e.Message);
                return;
            }
            Assert.False(true, "should have thrown an exception");
        }

        [Fact]
        public void QueryIfAbleToCreate()
        {
            Assert.False(sut.CanGet<SecondaryObject>());
            sut.Bind<ISimpleObject>().To<SimpleObjectImplementation>();
            Assert.True(sut.CanGet<SecondaryObject>());
        }

        [Fact]
        public void QueryCanCreateFactory()
        {
            Assert.False(sut.CanGet<Func<SecondaryObject>>());
            Assert.True(sut.CanGet<Func<ISimpleObject,SecondaryObject>>());
        }

        public class HasDefaultParam
        {
            public int P1 { get; }

            public HasDefaultParam(int p1 = 30)
            {
                P1 = p1;
            }
        }
        [Fact]
        public void CanDoDefaultParams()
        {
            Assert.Equal(30, sut.Get<HasDefaultParam>().P1);
        }

        public class OpenGeneric<T>
        {
            
        }
        [Fact]
        public void TestName()
        {
            Assert.Throws<IocException>(()=>sut.Get(typeof(OpenGeneric<>)));
        }

    }
}