using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Melville.IOC.BindingRequests;
using Melville.IOC.InjectionPolicies;

namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public sealed class AttemptDisposeRegistration : ForwardingActivationStrategy
    {
        private static IInterceptionRule rule = new AttemptDisposeRule();
        public AttemptDisposeRegistration(IActivationStrategy innerActivationStrategy) : base(innerActivationStrategy)
        {
        }

        public override object? Create(IBindingRequest bindingRequest) => 
            rule.Intercept(bindingRequest, InnerActivationStrategy.Create(bindingRequest));
    }
    
    public sealed class AttemptDisposeRule : IInterceptionRule
    {
        public object? Intercept(IBindingRequest request, object? source)
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
            else if (!bindingRequest.IocService.AllowDisposablesInGlobalScope)
            {
                throw new IocException($"Type {ret.GetType().Name} requires disposal but was created at global scope.");
            }
        }

        private static bool IsDisposableItem([NotNullWhen(true)]object? ret) =>
            ret is IDisposable || ret is IAsyncDisposable;

    }

}