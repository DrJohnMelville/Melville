using System;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.IocContainers
{
    public class ObjectFactory: IActivationOptions, IActivationStrategy
    {
        private IActivationStrategy activator;
        public bool CanCreate(IBindingRequest bindingRequest) => 
            activator.CanCreate(bindingRequest);

        public object? Create(IBindingRequest bindingRequest) => 
            activator.Create(bindingRequest);

        public SharingScope SharingScope() => activator.SharingScope();
        public bool ValidForRequest(IBindingRequest request) => activator.ValidForRequest(request);

        public ObjectFactory(IActivationStrategy activator)
        {
            this.activator = activator;
        }

        public IActivationOptions AsSingleton()
        {
            activator = WrapWithSingletonOnlyIfNecessary();
            return this;
        }

        private IActivationStrategy WrapWithSingletonOnlyIfNecessary() =>
            activator.SharingScope() == IocContainers.SharingScope.Singleton ? 
                activator:
                new SingletonActivationStrategy(activator);

        public IActivationOptions AsScoped()
        {
            activator = new ScopedActivationStrategy(activator);
            return this;
        }

        public IActivationOptions InNamedParamemter(string name)
        {
            activator = new ParameterNameCondition(activator, name);
            return this;
        }

        public IActivationOptions WhenConstructingType(Type? type)
        {
            activator = new TargetTypeCondition(activator, type);
            return this;
        }

        public IActivationOptions WithParameters(params object[] parameters)
        {
            activator = new AddParametersStrategy(activator, parameters);
            return this;
        }
    }
}