using System;
using System.Text.RegularExpressions;
using Melville.IOC.Activation;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;

namespace Melville.IOC.TypeResolutionPolicy
{
    public sealed class ConventionResolutionPolicy: ITypeResolutionPolicy{
        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request)
        {
            var targetType = ApplyConvention(request.DesiredType);
            return targetType == null ? null : 
                TypeActivatorFactory.CreateTypeActivator(targetType, ConstructorSelectors.MaximumArgumentCount);
        }

        private Type? ApplyConvention(Type source)
        {
            var match = Regex.Match(source.FullName??"Not a real Type", @"^(.+[\.\+])I([^\.\+]+)$");
            if (!match.Success) return null;
            var target = source.Assembly.GetType(match.Groups[1].Value + match.Groups[2].Value);
            if (target == null) return null;
            if (!source.IsAssignableFrom(target)) return null;
            if (!ActivatableTypesPolicy.IsActivatable(target)) return null;
            return target;
        }
    }
}