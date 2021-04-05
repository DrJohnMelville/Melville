using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;
using Xunit;

namespace Melville.IOC.Test.IocContainers
{
    public class CreateTypeActivatorTest
    {
        public class TwoNonOptimalConstructors
        {
            public TwoNonOptimalConstructors(int x){}
            public TwoNonOptimalConstructors(string x){}
            public TwoNonOptimalConstructors(string x, int y){}
        }

        [Fact]
        public void FindsMostNumerousConstructor()
        {
            TypeActivatorFactory.CreateTypeActivator(typeof(TwoNonOptimalConstructors),
                ConstructorSelectors.EmulateDotNet); // should not throw
        }
    }
}