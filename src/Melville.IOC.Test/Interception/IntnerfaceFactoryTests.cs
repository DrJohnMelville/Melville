using Melville.Ioc.Interception.InterfaceFactories;
using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.Interception;

public class IntnerfaceFactoryTests
{
    public class BO1{
        public BO1(string p)
        {
            P = p;
        }

        public string P { get; }
    }

    public interface IFactory
    {
        BO1 CreateBO(string p);
    }
        
    private readonly IocContainer sut = new IocContainer();

    [Fact]
    public void FactoryWorks()
    {
        sut.Bind<IFactory>().ToFactory();
        var fact = sut.Get<IFactory>();
        Assert.Equal("Foo", fact.CreateBO("Foo").P);
    }
}