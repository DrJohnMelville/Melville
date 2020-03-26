using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Melville.IOC.InjectionPolicies;

namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public sealed class AttemptDisposeRegistration : ForwardingActivationStrategy
    {
        private static IInjectionRule rule = new AttemptDisposeRule();
        public AttemptDisposeRegistration(IActivationStrategy innerActivationStrategy) : base(innerActivationStrategy)
        {
        }

        public override object? Create(IBindingRequest bindingRequest)
        {
            return rule.Inject(bindingRequest, InnerActivationStrategy.Create(bindingRequest));
        }
    }
    
    public sealed class AttemptDisposeRule : IInjectionRule
    {
        public object? Inject(IBindingRequest request, object? source)
        {
            if (IsDisposableItem(source))
            {
                RegisterDisposal(source, request);
            }

            return source;
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