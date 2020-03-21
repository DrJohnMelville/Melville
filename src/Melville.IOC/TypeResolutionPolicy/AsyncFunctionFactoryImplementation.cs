using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Melville.IOC.IocContainers;

namespace Melville.IOC.TypeResolutionPolicy
{
    public interface IAsyncFunctionFactoryImplementation
    {
        public void SetRequest(IBindingRequest request);
        void ClearCache();
        object CreateStatic(object[] parameters, IBindingRequest parentRequest);
    }

    public enum FactoryState
    {
        NotCreatedYet = 0,
        StaticCreationDone = 1,
        AsyncInitializationDone = 2
    };

    public static class RegisterCachedAsync
    {
        public static IActivationOptions BindAsyncFactory<T>(this IBindableIocService service) =>
            service.Bind<AsyncFunctionFactoryImplementation<T>>().ToSelf();

    }

    public class AsyncFunctionFactoryImplementation<T>: IAsyncFunctionFactoryImplementation
    {
        private IBindingRequest? request;
        // this is a hack to allow both nullable and nonnullable types
        [MaybeNull]
        private T datum = default(T)!;
        private readonly SemaphoreSlim initializationIsInProcess = new SemaphoreSlim(1);
        private volatile FactoryState state = FactoryState.NotCreatedYet;

        public void SetRequest(IBindingRequest request) => this.request = request;

        public async Task<T> Create(object[] parameters)
        {
            await EnsureInitialized(parameters, FetchAndInitializeDatum);
            return datum;
        }

        public void ClearCache() => state = FactoryState.NotCreatedYet;
        
        private async Task EnsureInitialized(object[] parameters, Func<object[], Task> computeValue)
        {
            if (state != FactoryState.AsyncInitializationDone)
            {
                await initializationIsInProcess.WaitAsync();
                try
                {
                    if (state != FactoryState.AsyncInitializationDone)
                    {
                        await computeValue(parameters);
                    }
                }
                finally
                {
                    initializationIsInProcess.Release();
                }
            }
        }

        //Member creation pattern
        private async Task FetchAndInitializeDatum(object[] parameters)
        {
            // make sure we do not get a torn request because SetRequest gets called on another thread.
            var localRequest = request;
            if (localRequest == null) throw new InvalidOperationException("Binding Request has not been set.");
            datum = FetchDatumIfNeeded(localRequest);
            if (datum == null) throw new InvalidOperationException("Failed to fetch datum");
            if (localRequest.Run(datum, "Create", parameters) is Task task) await task;
            state = FactoryState.AsyncInitializationDone;
        }

        private T FetchDatumIfNeeded(IBindingRequest localRequest)
        {
            if (state != FactoryState.NotCreatedYet) return datum;
            state = FactoryState.StaticCreationDone;
            return (T) (localRequest.IocService.Get(localRequest) ?? 
                        new InvalidOperationException("Failed to fetch datum"));
        }

        //static Creation Patern
        public object CreateStatic(object[] parameters, IBindingRequest parentRequest) => 
            CreateFromStaticImplementation(parameters, parentRequest);

        private async Task<T> CreateFromStaticImplementation(object[] parameters, IBindingRequest parentRequest)
        {
            request = parentRequest;
            await EnsureInitialized(parameters, StaticConstructor);
            return datum;
        }

        private async Task StaticConstructor(object[] parameters)
        {
            // make sure we do not get a torn request because SetRequest gets called on another thread.
            var localRequest = request;
            if (localRequest == null) throw new InvalidOperationException("Binding Request has not been set.");
            if (!(localRequest.RunStatic(typeof(T), "Create", parameters) is Task<T> creatorTask))
            {
                throw new InvalidProgramException("The Type policy should have guaranteed this function returns Task<T>");
            }
            datum = await creatorTask ;
        }
    }
}