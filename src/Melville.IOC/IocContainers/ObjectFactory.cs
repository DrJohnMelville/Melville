using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Melville.IOC.IocContainers
{
    public class ObjectFactory: IActivationOptions, IActivationStrategy
    {
        private IActivationStrategy activator;
        public bool CanCreate(IBindingRequest bindingRequest) => 
            activator.CanCreate(bindingRequest);

        public object? Create(IBindingRequest bindingRequest) => 
            activator.Create(bindingRequest);

        public Lifetime2 Lifetime() => activator.Lifetime();
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
            activator.Lifetime() == Lifetime2.Singleton ? 
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
    
    public interface IActivationStrategy
    {
        bool CanCreate(IBindingRequest bindingRequest);
        object? Create(IBindingRequest bindingRequest);
        Lifetime2 Lifetime();
        bool ValidForRequest(IBindingRequest request);
        void CreateMany(IBindingRequest bindingRequest, Func<object?, int> accumulator) =>
            accumulator(Create(bindingRequest));
    }

    public class ForwardingActivationStrategy : IActivationStrategy
    {
        private IActivationStrategy inner;

        public ForwardingActivationStrategy(IActivationStrategy inner)
        {
            this.inner = inner;
        }

        public bool CanCreate(IBindingRequest bindingRequest) => 
            inner.CanCreate(bindingRequest);

        public virtual object? Create(IBindingRequest bindingRequest) => 
            inner.Create(bindingRequest);
        public virtual Lifetime2 Lifetime() => inner.Lifetime();
        public virtual bool ValidForRequest(IBindingRequest request) => inner.ValidForRequest(request);
    }
    
    public class ScopedActivationStrategy : ForwardingActivationStrategy
    {
        public ScopedActivationStrategy(IActivationStrategy inner): base(inner)
        {
            if (inner.Lifetime() != Lifetime2.Transient)
            {
                throw new IocException("Bindings may only specify at most one lifetime.");
            }
        }

        public override Lifetime2 Lifetime() => Lifetime2.Scoped;
    
        private IScope Scope(IBindingRequest req) => 
            (req.IocService is IScope scope) ?scope: throw new IocException($"Attempted to create a scoped {req.DesiredType.Name} outside of a scope.");

        public override object? Create(IBindingRequest bindingRequest)=>
            Scope(bindingRequest).TryGetValue(this, out var ret) ? ret : 
                RecordScopedValue(bindingRequest, base.Create(bindingRequest));

        private object? RecordScopedValue(IBindingRequest bindingRequest, object? create)
        {
            Scope(bindingRequest).SetScopeValue(this, create);
            return create;
        }
    }
    public class SingletonActivationStrategy : ForwardingActivationStrategy
    {
        //these two fields must be volatile for the double check and lock pattern to work
        private volatile object? value;
        private volatile bool valueExists;

        public SingletonActivationStrategy(IActivationStrategy innerStrategy): base(innerStrategy)
        { 
            if (innerStrategy.Lifetime() != Lifetime2.Transient)
            {
                throw new IocException("Bindings may only specify at most one lifetime.");
            }
        }
        public override Lifetime2 Lifetime() => Lifetime2.Singleton;

        public override object? Create(IBindingRequest bindingRequest)
        {
            CreateValueExactlyOnceForAllThreads(bindingRequest);
            return value;
        }

        private void CreateValueExactlyOnceForAllThreads(IBindingRequest bindingRequest)
        {
            //the double check and lock pattern relies on value and value exists being volitile fields
            if (!valueExists)
            {
                lock (this)
                {
                    if (!valueExists)
                    {
                        value = ComputeSingleValue(bindingRequest);
                        valueExists = true;
                    }
                }
            }
        }

        private object? ComputeSingleValue(IBindingRequest bindingRequest)
        {
            var oldScope = ExchangeRequestScope(bindingRequest, bindingRequest.IocService.GlobalScope);
            var ret = base.Create(bindingRequest);
            ExchangeRequestScope(bindingRequest, oldScope);
            return ret;
        }
        /// <summary>
        /// A singleton object cannot depend on a scoped object because it "captures" the scoped object and may access
        /// if after the scope closes and destroys the object.  To prevent this we eliminate the scope for a singleton
        /// evaluation and then put it back in case the singleton is part of a multiple object activation 
        /// </summary>
        private IIocService ExchangeRequestScope(IBindingRequest request, IIocService newScope)
        {
            var ret = request.IocService;
            request.IocService = newScope;
            return ret;
        }
    }

    public class TypeActivationStrategy: IActivationStrategy
    {
        private readonly ParameterInfo[] paramTypes;
        private readonly Func<object?[], object> activator;

        public Lifetime2 Lifetime() => Lifetime2.Transient;
        public bool ValidForRequest(IBindingRequest request) => true;
        public bool CanCreate(IBindingRequest bindingRequest) => 
            bindingRequest.IocService.CanGet(ComputeDependencies(bindingRequest));

        public object? Create(IBindingRequest bindingRequest) => 
            activator(bindingRequest.IocService.Get(ComputeDependencies(bindingRequest)));

        private List<IBindingRequest> ComputeDependencies(IBindingRequest bindingRequest) =>
            paramTypes
                .Select(bindingRequest.CreateSubRequest)
                .ToList();

        public TypeActivationStrategy(Func<object?
            [], object> activator, ParameterInfo[] paramTypes)
        {
            this.activator = activator;
            this.paramTypes = paramTypes;
        }
    }
    
    public sealed class ConstantActivationStrategy: IActivationStrategy
    {
        private readonly object? value;

        public ConstantActivationStrategy(object? value)
        {
            this.value = value;
        }

        public Lifetime2 Lifetime() => Lifetime2.Singleton;
        public bool ValidForRequest(IBindingRequest request) => true;
        public bool CanCreate(IBindingRequest bindingRequest) => true;
        public object? Create(IBindingRequest bindingRequest) => value;
    }
}