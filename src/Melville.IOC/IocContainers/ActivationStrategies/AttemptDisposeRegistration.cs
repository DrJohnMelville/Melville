using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public sealed class AttemptDisposeRegistration : ForwardingActivationStrategy
    {
        public AttemptDisposeRegistration(IActivationStrategy innerActivationStrategy) : base(innerActivationStrategy)
        {
        }

        public override object? Create(IBindingRequest bindingRequest)
        {
            var ret = InnerActivationStrategy.Create(bindingRequest);
            TryRegisterDisposal(ret, bindingRequest);
            return ret;
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
}