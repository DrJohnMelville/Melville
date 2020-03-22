using System;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.IocContainers
{
    public class ObjectFactory: ForwardingActivationStrategy
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
        public ObjectFactory(IActivationStrategy innerActivationStrategy) :
            base(new AttemptDisposeRegistration(innerActivationStrategy))
        {
        }
    }

    public class ObjectFactory<T>: ObjectFactory, IActivationOptions<T>
    {


        public ObjectFactory(IActivationStrategy innerActivationStrategy):base(innerActivationStrategy)
        {
        }

        public IActivationOptions<T> AsSingleton() => AddActivationStrategy(WrapWithSingletonOnlyIfNecessary());

        private IActivationStrategy WrapWithSingletonOnlyIfNecessary() =>
            InnerActivationStrategy.SharingScope() == IocContainers.SharingScope.Singleton ? 
                InnerActivationStrategy:
                new SingletonActivationStrategy(InnerActivationStrategy);

        public IActivationOptions<T> AsScoped() => 
            AddActivationStrategy(new ScopedActivationStrategy(InnerActivationStrategy));

        public IActivationOptions<T> WithParameters(params object[] parameters) => 
            AddActivationStrategy(new AddParametersStrategy(InnerActivationStrategy, parameters));

        public IActivationOptions<T> DoNotDispose() => AddActivationStrategy(
            new ForbidDisposalStrategy(InnerActivationStrategy, true));

        public IActivationOptions<T> DisposeIfInsideScope() => AddActivationStrategy(
            new ForbidDisposalStrategy(InnerActivationStrategy, false));

        public IActivationOptions<T> When(Func<IBindingRequest, bool> predicate) => AddActivationStrategy(
            new LambdaCondition(InnerActivationStrategy, predicate));

        private IActivationOptions<T> AddActivationStrategy(IActivationStrategy newStrategy)
        {
            InnerActivationStrategy = newStrategy;
            return this;
        }
    }

}