using System.Collections.Generic;
using System.Linq;
using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.IocContainers;

public class MultipleResolutionTest
{
    public interface IMultipleImplementation2
    {
    }

    public interface IMultipleImplementation: IMultipleImplementation2 
    {
    }

    public class Implementation1 : IMultipleImplementation{}
    public class Implementation2: IMultipleImplementation{}
    public class Implementation3: IMultipleImplementation{}

    public class ImplementationList : IMultipleImplementation
    {
        public ImplementationList(IList<IMultipleImplementation> impls)
        {
            Impls = impls;
        }

        public IList<IMultipleImplementation> Impls { get; }
    }

    private readonly IocContainer sut = new IocContainer();

    [Fact]
    public void MultipleRegistrationBindsLastOne()
    {
        sut.Bind<IMultipleImplementation>().To<Implementation1>();
        sut.Bind<IMultipleImplementation>().To<Implementation2>();
            
        Assert.True(sut.Get<IMultipleImplementation>() is Implementation2);
    }

    [Fact]
    public void IfNeededRegistrationIgnoresLastOne()
    {
        sut.Bind<IMultipleImplementation>().To<Implementation1>();
        sut.BindIfMNeeded<IMultipleImplementation>().To<Implementation2>();
            
        Assert.True(sut.Get<IMultipleImplementation>() is Implementation1);
    }
    [Fact]
    public void IfNeededRegistrationIgnoresLastOneIsPerInterface()
    {
        sut.Bind<IMultipleImplementation>().To<Implementation1>();
        sut.BindIfMNeeded<IMultipleImplementation>().And<IMultipleImplementation2>().To<Implementation2>();
            
        Assert.True(sut.Get<IMultipleImplementation>() is Implementation1);
        Assert.True(sut.Get<IMultipleImplementation2>() is Implementation2);
    }

    [Fact]
    public void MultipleResolutionBindNone()
    {
        var objects = sut.Get<IList<IMultipleImplementation>>();
            
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
        sut.Bind<IMultipleImplementation>().To<Implementation1>();
        sut.Bind<IMultipleImplementation>().To<Implementation2>();
        sut.Bind<IMultipleImplementation>().To<Implementation3>();

        var objects = sut.Get<IEnumerable<IMultipleImplementation>>().ToList();
        Assert.Equal(3, objects.Count);
        Assert.True(objects[0] is Implementation1);
        Assert.True(objects[1] is Implementation2);
        Assert.True(objects[2] is Implementation3);
    }
    [Fact]
    public void MultipleRegistrationBindsAllList()
    {
        sut.Bind<IMultipleImplementation>().To<Implementation1>();
        sut.Bind<IMultipleImplementation>().To<Implementation2>();
        sut.Bind<IMultipleImplementation>().To<Implementation3>();

        var objects = sut.Get<IList<IMultipleImplementation>>();
        Assert.Equal(3, objects.Count);
        Assert.True(objects[0] is Implementation1);
        Assert.True(objects[1] is Implementation2);
        Assert.True(objects[2] is Implementation3);
    }
    [Fact]
    public void MultipleRegistrationBindsAllCollection()
    {
        sut.Bind<IMultipleImplementation>().To<Implementation1>();
        sut.Bind<IMultipleImplementation>().To<Implementation2>();
        sut.Bind<IMultipleImplementation>().To<Implementation3>();

        var objects = sut.Get<ICollection<IMultipleImplementation>>().ToList();
        Assert.Equal(3, objects.Count);
        Assert.True(objects[0] is Implementation1);
        Assert.True(objects[1] is Implementation2);
        Assert.True(objects[2] is Implementation3);
    }

    public class MultiParam
    {
        public IMultipleImplementation Var1 { get; }
        public IMultipleImplementation Var2 { get; }

        public MultiParam(IMultipleImplementation var1, IMultipleImplementation var2)
        {
            Var1 = var1;
            Var2 = var2;
        }
    }
    public class SingleParam
    {
        public SingleParam(IMultipleImplementation single)
        {
            Single = single;
        }

        public IMultipleImplementation Single { get; }
    }
    [Fact]
    public void NamedResolution()
    {
        sut.Bind<IMultipleImplementation>().To<Implementation1>();
        sut.Bind<IMultipleImplementation>().To<Implementation2>().InNamedParameter("var2");
        var ret = sut.Get<MultiParam>();
        Assert.True(ret.Var1 is Implementation1);
        Assert.True(ret.Var2 is Implementation2);
    }

    [Fact]
    public void SingleResolutionRespectsName()
    {
        sut.Bind<IMultipleImplementation>().To<Implementation1>().InNamedParameter("var1");
        sut.Bind<IMultipleImplementation>().To<Implementation2>().InNamedParameter("var2");
        sut.Get<MultiParam>(); // succeeds using named parameters;
        Assert.Throws<IocException>(()=>sut.Get<IMultipleImplementation>()); // no unnamed binding so fails
        Assert.Throws<IocException>(()=>sut.Get<SingleParam>()); // no unnamed binding so fails
    }
    [Fact]
    public void SingleResolutionRespectsTargetType()
    {
        sut.Bind<IMultipleImplementation>().To<Implementation1>().WhenConstructingType<MultiParam>();
        var ret = sut.Get<MultiParam>(); // succeeds using Typed parameters;
        Assert.True(ret.Var1 is Implementation1);
        Assert.True(ret.Var2 is Implementation1);
        Assert.Throws<IocException>(()=>sut.Get<SingleParam>()); // no Untyped binding so fails
        Assert.Throws<IocException>(()=>sut.Get<IMultipleImplementation>()); // no Untyped binding so fails
    }

    [Fact]
    public void ConstructCompositePattern()
    {
        sut.Bind<IMultipleImplementation>().To<Implementation1>();
        sut.Bind<IMultipleImplementation>().To<Implementation2>();
        sut.Bind<IMultipleImplementation>().To<Implementation3>();
        sut.Bind<IMultipleImplementation>().To<ImplementationList>()
            .WhenNotConstructingType<ImplementationList>();
        var ret = sut.Get<IMultipleImplementation>();
        Assert.Equal(3, ((ImplementationList)ret).Impls.Count);
    }
}