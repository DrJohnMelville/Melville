using System;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.IocContainers
{
    public class ObjectFactory: ForwardingActivationStrategy, IActivationOptions
    {

        public static ObjectFactory? ForceToObjectFactory(IActivationStrategy? activationStrategy)
        {
            return activationStrategy switch
            {
                null => null,
                ObjectFactory factory  => factory,
                _=> new ObjectFactory(activationStrategy)
            };
        }

        public ObjectFactory(IActivationStrategy innerActivationStrategy):base(
            new AttemptDisposeRegistration(innerActivationStrategy))
        {
        }

        public IActivationOptions AsSingleton() => AddActivationStrategy(WrapWithSingletonOnlyIfNecessary());

        private IActivationStrategy WrapWithSingletonOnlyIfNecessary() =>
            InnerActivationStrategy.SharingScope() == IocContainers.SharingScope.Singleton ? 
                InnerActivationStrategy:
                new SingletonActivationStrategy(InnerActivationStrategy);

        public IActivationOptions AsScoped() => 
            AddActivationStrategy(new ScopedActivationStrategy(InnerActivationStrategy));

        public IActivationOptions InNamedParamemter(string name) => 
            AddActivationStrategy(new ParameterNameCondition(InnerActivationStrategy, name));

        public IActivationOptions WhenConstructingType(Type? type) => 
            AddActivationStrategy(new TargetTypeCondition(InnerActivationStrategy, type));

        public IActivationOptions WithParameters(params object[] parameters) => 
            AddActivationStrategy(new AddParametersStrategy(InnerActivationStrategy, parameters));

        public IActivationOptions DoNotDispose() => AddActivationStrategy(
            new ForbidDisposalStrategy(InnerActivationStrategy, true));

        public IActivationOptions DisposeIfInsideScope() => AddActivationStrategy(
            new ForbidDisposalStrategy(InnerActivationStrategy, false));

        private IActivationOptions AddActivationStrategy(IActivationStrategy newStrategy)
        {
            InnerActivationStrategy = newStrategy;
            return this;
        }
    }
}