using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers.ChildContainers;

public class ChildContainer : IocContainer
{
    public int Depth { get; }

    public ChildContainer(IBindableIocService parent, IIocService parentContext)
    {
        var paretCache = parent.ConfigurePolicy<CachedResolutionPolicy>();
        ConfigurePolicy<ISetBackupCache>().SetBackupCache(
            new CreateInContextPolicy(paretCache, parentContext));
        Depth = 1 + ((parent as ChildContainer)?.Depth ?? 1);
    }


    public class CreateInContextPolicy(
        ITypeResolutionPolicy inner,
        IIocService parentContext) : ITypeResolutionPolicy
    {
        /// <inheritdoc />
        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request)
        {
            return inner.ApplyResolutionPolicy(RemoveSingletonParentRequest.StripOuterSingletonScope(request)) is
                { } strategy
                ? new StrategyInContext(strategy, parentContext)
                : null;
        }

    }


    public class StrategyInContext(
        IActivationStrategy strategy,
        IIocService parentContext) : IActivationStrategy
    {
        private IBindingRequest NewRequest(IBindingRequest oldRequest) =>
            parentContext.SwitchRequestToMyContext(oldRequest);

        /// <inheritdoc />
        public bool CanCreate(IBindingRequest bindingRequest) =>
            strategy.CanCreate(NewRequest(bindingRequest));

        /// <inheritdoc />
        public object? Create(IBindingRequest bindingRequest) =>
            strategy.Create(NewRequest(bindingRequest));

        /// <inheritdoc />
        public SharingScope SharingScope() => strategy.SharingScope();

        /// <inheritdoc />
        public bool ValidForRequest(IBindingRequest request) => strategy.ValidForRequest(
            NewRequest(request));
    }
}