namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public class SingletonActivationStrategy : ForwardingActivationStrategy
    {
        //these two fields must be volatile for the double check and lock pattern to work
        private volatile object? value;
        private volatile bool valueExists;

        public SingletonActivationStrategy(IActivationStrategy innerActivationStrategyStrategy): base(innerActivationStrategyStrategy)
        { 
            if (innerActivationStrategyStrategy.SharingScope() != IocContainers.SharingScope.Transient)
            {
                throw new IocException("Bindings may only specify at most one lifetime.");
            }
        }
        public override SharingScope SharingScope() => IocContainers.SharingScope.Singleton;

        public override object? Create(IBindingRequest bindingRequest)
        {
            CreateValueExactlyOnceForAllThreads(bindingRequest);
            return value;
        }

        private void CreateValueExactlyOnceForAllThreads(IBindingRequest bindingRequest)
        {
            //the double check and lock pattern relies on value and valueExists being volitile fields
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
            var oldScope = ExchangeRequestScope(bindingRequest, bindingRequest.IocService.GlobalScope());
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

        public static IActivationStrategy EnsureSingleton(IActivationStrategy inner) =>
            inner.SharingScope() == IocContainers.SharingScope.Singleton
                ? inner
                : new SingletonActivationStrategy(inner);
    }
}