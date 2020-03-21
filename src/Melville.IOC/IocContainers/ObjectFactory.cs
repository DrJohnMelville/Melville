using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
            new ForbidDisposalStrategy(InnerActivationStrategy, DisposalState.DisposalDone));

        public IActivationOptions DisposeIfInsideScope() => AddActivationStrategy(
            new ForbidDisposalStrategy(InnerActivationStrategy, DisposalState.DisposeOptional));

        private IActivationOptions AddActivationStrategy(IActivationStrategy newStrategy)
        {
            InnerActivationStrategy = newStrategy;
            return this;
        }
    }

    public sealed class AttemptDisposeRegistration : ForwardingActivationStrategy
    {
        public AttemptDisposeRegistration(IActivationStrategy innerActivationStrategy) : base(innerActivationStrategy)
        {
        }

        public override (object? Result, DisposalState DisposalState) Create(IBindingRequest bindingRequest)
        {
            var (ret, _) = InnerActivationStrategy.Create(bindingRequest);
            TryRegisterDisposal(ret, bindingRequest);
            return (ret, DisposalState.DisposalDone);
        }

        private void TryRegisterDisposal(object ret, IBindingRequest bindingRequest)
        {
            if (IsDisposableItem(ret))
            {
                RegisterDisposal(ret, bindingRequest);
            }
        }

        private void RegisterDisposal(object ret, IBindingRequest bindingRequest)
        {
            if (bindingRequest.IocService.ScopeList().OfType<IRegisterDispose>().FirstOrDefault() is {} reg)
            {
                reg.RegisterForDispose(ret);
            }
            else
            {
                throw new IocException($"Type {ret.GetType().Name} requires disposal but was created at global scope.");
            }
        }

        private static bool IsDisposableItem([NotNullWhen(true)]object? ret) =>
            ret is IDisposable || ret is IAsyncDisposable;

    }

    public sealed class ForbidDisposalStrategy : ForwardingActivationStrategy
    {
        private readonly bool forbidDisposeEvenIfInScope;

        public ForbidDisposalStrategy(IActivationStrategy inner, DisposalState newDisposalState): base(inner)
        {
            forbidDisposeEvenIfInScope = newDisposalState == DisposalState.DisposalDone;
        }

        public override (object? Result, DisposalState DisposalState) Create(IBindingRequest bindingRequest)
        {
            if ((!bindingRequest.IocService.ScopeList().OfType<IRegisterDispose>().Any()) ||
                forbidDisposeEvenIfInScope)
            {
                SetDisposalContextToAContextThatWillNeverGetDisposed(bindingRequest);
            }

            var (ret, _) = InnerActivationStrategy.Create(bindingRequest);
            return (ret, DisposalState.DisposalDone);
        }

        private static void SetDisposalContextToAContextThatWillNeverGetDisposed(IBindingRequest bindingRequest)
        {
            bindingRequest.IocService = new DisposableIocService(bindingRequest.IocService);
        }
    }
}