using System.Collections.Generic;
using System.Linq;
using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.IocContainers
{
    public class MultipleResolutionTest
    {
        public interface IMultipleImplementation2
        {
        }

        public interface IIMultipleImplementation: IMultipleImplementation2 
        {
        }

        public class Implementation1 : IIMultipleImplementation{}
        public class Implementation2: IIMultipleImplementation{}
        public class Implementation3: IIMultipleImplementation{}

        private readonly IocContainer sut = new IocContainer();

        [Fact]
        public void MultipleRegistrationBindsLastOne()
        {
            sut.Bind<IIMultipleImplementation>().To<Implementation1>();
            sut.Bind<IIMultipleImplementation>().To<Implementation2>();
            
            Assert.True(sut.Get<IIMultipleImplementation>() is Implementation2);
        }

        [Fact]
        public void IfNeededRegistrationIgnoresLastOne()
        {
            sut.Bind<IIMultipleImplementation>().To<Implementation1>();
            sut.BindIfMNeeded<IIMultipleImplementation>().To<Implementation2>();
            
            Assert.True(sut.Get<IIMultipleImplementation>() is Implementation1);
        }
        [Fact]
        public void IfNeededRegistrationIgnoresLastOneIsPerInterface()
        {
            sut.Bind<IIMultipleImplementation>().To<Implementation1>();
            sut.BindIfMNeeded<IIMultipleImplementation>().And<IMultipleImplementation2>().To<Implementation2>();
            
            Assert.True(sut.Get<IIMultipleImplementation>() is Implementation1);
            Assert.True(sut.Get<IMultipleImplementation2>() is Implementation2);
        }

        [Fact]
        public void MultipleResolutionBindNone()
        {
            var objects = sut.Get<IList<IIMultipleImplementation>>();
            
            Assert.Empty(objects);
        }
        [Fact]
        public void MultipleResolutionBindImplicit()
        {
            var objects = sut.Get<IList<Implementation1>>();
            
            Assert.Single(objects);
            Assert.True(objects[0] is Implementation1);
        }

        [Fact]
        public void MultipleRegistrationBindsAll()
        {
            sut.Bind<IIMultipleImplementation>().To<Implementation1>();
            sut.Bind<IIMultipleImplementation>().To<Implementation2>();
            sut.Bind<IIMultipleImplementation>().To<Implementation3>();

            var objects = sut.Get<IEnumerable<IIMultipleImplementation>>().ToList();
            Assert.Equal(3, objects.Count);
            Assert.True(objects[0] is Implementation1);
            Assert.True(objects[1] is Implementation2);
            Assert.True(objects[2] is Implementation3);
        }
        [Fact]
        public void MultipleRegistrationBindsAllList()
        {
            sut.Bind<IIMultipleImplementation>().To<Implementation1>();
            sut.Bind<IIMultipleImplementation>().To<Implementation2>();
            sut.Bind<IIMultipleImplementation>().To<Implementation3>();

            var objects = sut.Get<IList<IIMultipleImplementation>>();
            Assert.Equal(3, objects.Count);
            Assert.True(objects[0] is Implementation1);
            Assert.True(objects[1] is Implementation2);
            Assert.True(objects[2] is Implementation3);
        }
        [Fact]
        public void MultipleRegistrationBindsAllCollection()
        {
            sut.Bind<IIMultipleImplementation>().To<Implementation1>();
            sut.Bind<IIMultipleImplementation>().To<Implementation2>();
            sut.Bind<IIMultipleImplementation>().To<Implementation3>();

            var objects = sut.Get<ICollection<IIMultipleImplementation>>().ToList();
            Assert.Equal(3, objects.Count);
            Assert.True(objects[0] is Implementation1);
            Assert.True(objects[1] is Implementation2);
            Assert.True(objects[2] is Implementation3);
        }

        public class MultiParam
        {
            public IIMultipleImplementation Var1 { get; }
            public IIMultipleImplementation Var2 { get; }

            public MultiParam(IIMultipleImplementation var1, IIMultipleImplementation var2)
            {
                Var1 = var1;
                Var2 = var2;
            }
        }
        public class SingleParam
        {
            public SingleParam(IIMultipleImplementation single)
            {
                Single = single;
            }

            public IIMultipleImplementation Single { get; }
        }
        [Fact]
        public void NamedResolution()
        {
            sut.Bind<IIMultipleImplementation>().To<Implementation1>();
            sut.Bind<IIMultipleImplementation>().To<Implementation2>().InNamedParamemter("var2");
            var ret = sut.Get<MultiParam>();
            Assert.True(ret.Var1 is Implementation1);
            Assert.True(ret.Var2 is Implementation2);
        }

        [Fact]
        public void SingleResolutionRespectsName()
        {
            sut.Bind<IIMultipleImplementation>().To<Implementation1>().InNamedParamemter("var1");
            sut.Bind<IIMultipleImplementation>().To<Implementation2>().InNamedParamemter("var2");
            sut.Get<MultiParam>(); // succeeds using named parameters;
            Assert.Throws<IocException>(()=>sut.Get<IIMultipleImplementation>()); // no unnamed binding so fails
            Assert.Throws<IocException>(()=>sut.Get<SingleParam>()); // no unnamed binding so fails
        }
        [Fact]
        public void SingleResolutionRespectsTargetType()
        {
            sut.Bind<IIMultipleImplementation>().To<Implementation1>().WhenConstructingType<MultiParam>();
            var ret = sut.Get<MultiParam>(); // succeeds using Typed parameters;
            Assert.True(ret.Var1 is Implementation1);
            Assert.True(ret.Var2 is Implementation1);
            Assert.Throws<IocException>(()=>sut.Get<SingleParam>()); // no Untyped binding so fails
            Assert.Throws<IocException>(()=>sut.Get<IIMultipleImplementation>()); // no Untyped binding so fails
        }
    }
}