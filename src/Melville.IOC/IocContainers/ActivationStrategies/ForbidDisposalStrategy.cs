using System.Linq;

namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public sealed class ForbidDisposalStrategy : ForwardingActivationStrategy
    {
        private readonly bool forbidDisposeEvenIfInScope;

        public ForbidDisposalStrategy(IActivationStrategy inner, bool forbidDisposeEvenIfInScope): base(inner)
        {
            this.forbidDisposeEvenIfInScope = forbidDisposeEvenIfInScope;
        }

        public override object? Create(IBindingRequest bindingRequest)
        {
            if ((!bindingRequest.IocService.ScopeList().OfType<IRegisterDispose>().Any()) ||
                forbidDisposeEvenIfInScope)
            {
                SetDisposalContextToAContextThatWillNeverGetDisposed(bindingRequest);
            }

            return InnerActivationStrategy.Create(bindingRequest);
        }

        private static void SetDisposalContextToAContextThatWillNeverGetDisposed(IBindingRequest bindingRequest)
        {
            bindingRequest.IocService = new DisposableIocService(bindingRequest.IocService);
        }
    }
}