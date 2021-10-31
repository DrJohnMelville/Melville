using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.Interception;

public class InterceptionTest
{
    public interface ITarget1 { }
    public interface ITarget2 { }

    public class Target1 : ITarget1, ITarget2 { }
    public class Target2 : ITarget1, ITarget2 { }

    public class Wrapper1 : ITarget1
    {
        public ITarget1 Inner { get;}

        public Wrapper1(ITarget1 inner)
        {
            Inner = inner;
        }
    }
        
    private readonly IocContainer sut = new IocContainer();

    [Fact]
    public void SimpleInerceptByType()
    {
        sut.Intercept((ITarget1 i) => new Wrapper1(i));
        Assert.True(sut.Get<ITarget1>() is Wrapper1 wrap && wrap.Inner is Target1);
        Assert.True(sut.Get<Target1>() is Target1); 
        // even though Target 1 is an ITarget1 I can only wrap the interface I ask 1
    }

    [Fact]
    public void IntnerceptSpeceficTypeAsInterface()
    {
        sut.InterceptSpecificType((Target1 i) => (ITarget1)new Wrapper1(i));
        sut.Bind<ITarget1>().To<Target1>();
        Assert.True(sut.Get<ITarget1>() is Wrapper1);
        sut.Bind<ITarget1>().To<Target2>(); // override binding.
        Assert.True(sut.Get<ITarget1>() is Target2);
    }
}