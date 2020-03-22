using System;
using System.Linq;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.IocContainers
{
    public class ObjectFactory: ForwardingActivationStrategy
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
        public ObjectFactory(IActivationStrategy innerActivationStrategy) :
            base(new AttemptDisposeRegistration(innerActivationStrategy))
        {
        }
    }

    public class ObjectFactory<T>: ObjectFactory, IActivationOptions<T>
    {


        public ObjectFactory(IActivationStrategy innerActivationStrategy):base(innerActivationStrategy)
        {
        }

        public IActivationOptions<T> AddActivationStrategy(Func<IActivationStrategy, IActivationStrategy> newStrategy)
        {
            InnerActivationStrategy = newStrategy(InnerActivationStrategy);
            return this;
        }
        
        public IActivationOptions<T> WrapWith<TWrapper>() where TWrapper : T =>
            ((IActivationOptions<T>)this).WrapWith((item, request) => 
                (T) request.IocService.Get(request.CreateSubRequest(typeof(TWrapper), item)));
        public IActivationOptions<T> WrapWith<TWrapper>(params  object[] parameters) where TWrapper : T =>
            ((IActivationOptions<T>)this).WrapWith((item, request) => 
                (T) request.IocService.Get(request.CreateSubRequest(typeof(TWrapper), 
                    parameters.Prepend(item).ToArray())));

    }

}