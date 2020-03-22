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

        public IActivationOptions WithParameters(params object[] parameters) => 
            AddActivationStrategy(new AddParametersStrategy(InnerActivationStrategy, parameters));

        public IActivationOptions DoNotDispose() => AddActivationStrategy(
            new ForbidDisposalStrategy(InnerActivationStrategy, true));

        public IActivationOptions DisposeIfInsideScope() => AddActivationStrategy(
            new ForbidDisposalStrategy(InnerActivationStrategy, false));

        public IActivationOptions When(Func<IBindingRequest, bool> predicate) => AddActivationStrategy(
            new LambdaCondition(InnerActivationStrategy, predicate));

        private IActivationOptions AddActivationStrategy(IActivationStrategy newStrategy)
        {
            InnerActivationStrategy = newStrategy;
            return this;
        }
        
    }
}