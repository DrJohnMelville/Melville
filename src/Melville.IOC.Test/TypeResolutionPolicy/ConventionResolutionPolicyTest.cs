using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.TypeResolutionPolicy;

public class ConventionResolutionPolicyTest
{
    private readonly IocContainer sut = new IocContainer();
    public interface IIConventionallyNamed { }
    public interface IConventionallyNamed { }
    public class ConventionallyNamed: IConventionallyNamed, IIConventionallyNamed { }

    [Fact]
    public void CreateFRomConventionWithoutBinding()
    {
        Assert.True(sut.Get<IConventionallyNamed>() is  ConventionallyNamed);
    }

    [Fact]
    public void ConventionMustBeExact()
    {
        Assert.Throws<IocException>(() => sut.Get<IIConventionallyNamed>());
    }
        
    public interface ICn2 { }
    public class Cn2 { } // does not implement ICN2

    [Fact]
    public void TargetMustImplementInterface()
    {
        Assert.Throws<IocException>(() => sut.Get<ICn2>());
    }
}