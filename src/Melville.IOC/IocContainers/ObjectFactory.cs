using System;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.IocContainers
{
    public class ObjectFactory: ForwardingActivationStrategy, IActivationOptions
    {

        public ObjectFactory(IActivationStrategy innerActivationStrategy):base(innerActivationStrategy)
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
            new DisposalStrategy(InnerActivationStrategy, DisposalState.DisposalDone));

        public IActivationOptions DisposeIfInsideScope() => AddActivationStrategy(
            new DisposalStrategy(InnerActivationStrategy, DisposalState.DisposeOptional));

        private IActivationOptions AddActivationStrategy(IActivationStrategy newStrategy)
        {
            InnerActivationStrategy = newStrategy;
            return this;
        }
    }

    public sealed class DisposalStrategy : ForwardingActivationStrategy
    {
        private readonly DisposalState newDisposalState;

        public DisposalStrategy(IActivationStrategy inner, DisposalState newDisposalState): base(inner)
        {
            this.newDisposalState = newDisposalState;
        }

        public override (object? Result, DisposalState DisposalState) Create(IBindingRequest bindingRequest)
        {
            var (ret, _) = InnerActivationStrategy.Create(bindingRequest);
            return (ret, newDisposalState);
        }
    }
}