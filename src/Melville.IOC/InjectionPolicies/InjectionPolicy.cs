using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.InjectionPolicies
{
    public interface IInjectionPolicy
    {
        public IActivationStrategy Inject(IActivationStrategy inner);
    }
    public class DefaultInjectionPolicy: IInjectionPolicy
    {
        public IActivationStrategy Inject(IActivationStrategy inner)
        {
            return new AttemptDisposeRegistration(inner);
        }
    }
}