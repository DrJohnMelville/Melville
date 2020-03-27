using System;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.TypeResolutionPolicy
{
    public class ArgumentBindingPolicy: ITypeResolutionPolicy
    {
        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request)
        {
            var objects = request.ExtraParamsFromParent;
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] is {} ret && ObjectFillsRequest(ret, request))
                {
                    RemoveArgumentSoNooneElseCanUseIt(objects, i);
                    return new ConstantActivationStrategy(ret);
                }
            }
            return null;
        }

        private static void RemoveArgumentSoNooneElseCanUseIt(object?[] objects, int i) => objects[i] = null;

        private bool ObjectFillsRequest(object value, IBindingRequest request) =>
            request.DesiredType.IsInstanceOfType(value) ||
            (value is ReplaceLiteralArgumentWithNonsenseValue fake && fake.CanAssignTo(request.DesiredType));
    }

    /// <summary>
    /// When checking if a function based factory can be created we need to check if the function could actually be
    /// invoked.  To do this we need to remove the functions arguments from the types that need to be resolved for
    /// a successful invocation.  LiteralBindingPolicy special cases this class and allows it to replace any class
    /// that CanAssignTo replaces.  Since this class does not actually inherit from fakedType, this will cause a
    /// typecast error if ever invoked, but since the CanGet branch never invokes the activation strategy this
    /// technique is completely safe.
    /// </summary>
    public class ReplaceLiteralArgumentWithNonsenseValue
    {
        private readonly Type fakedType;

        public ReplaceLiteralArgumentWithNonsenseValue(Type fakedType)
        {
            this.fakedType = fakedType;
        }

        public bool CanAssignTo(Type targetType) => targetType.IsAssignableFrom(fakedType);
    }
}