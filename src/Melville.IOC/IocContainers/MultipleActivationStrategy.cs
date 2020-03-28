using System;
using System.Collections.Generic;
using System.Linq;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.IocContainers
{
    public class MultipleActivationStrategy : IActivationStrategy
    {
        private readonly List<IActivationStrategy> strateies = new List<IActivationStrategy>();

        public MultipleActivationStrategy(IActivationStrategy strategy) => strateies.Add(strategy);
        
        private IActivationStrategy? SelectActivator(IBindingRequest bindingRequest) => 
            strateies.LastOrDefault(i => i.ValidForRequest(bindingRequest));
        
        public void AddStrategy(IActivationStrategy strategy) => strateies.Add(strategy);

        public bool CanCreate(IBindingRequest bindingRequest) =>
            SelectActivator(bindingRequest)?.CanCreate(bindingRequest) ?? false;
         
        public object? Create(IBindingRequest bindingRequest)=>
            (SelectActivator(bindingRequest)??
                throw new IocException($"No binding for {bindingRequest.DesiredType.Name} is valid in this context.")
            ).Create(bindingRequest);

        public void CreateMany(IBindingRequest bindingRequest,
            Func<object?, int> accumulator)
        {
            foreach (var strategy in strateies)
            {
                var request = bindingRequest.Clone();
                if (!strategy.ValidForRequest(request)) continue;
                accumulator(strategy.Create(request) ?? throw new IocException("Type resolved to null"));
            }
        }


        public SharingScope SharingScope() => strateies.Select(i => i.SharingScope()).Min();
        public bool ValidForRequest(IBindingRequest request) => SelectActivator(request) != null;
    }
}