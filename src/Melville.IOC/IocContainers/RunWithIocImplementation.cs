using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers
{
    public static class RunWithIocImplementation
    {
        public static object? RunStatic(this IBindingRequest request, Type target, string methodName,
            params object[] parameters) =>
          request.CreateSubRequest(target, parameters).RunStatic(target, methodName);
        public static object? RunStatic(this IBindingRequest request, Type target, string methodName)
        {
            return RunStaticOrMemberMethod(request, null, methodName, target, StaticMethodSearchCriteria);
        }
        public static object? Run(this IBindingRequest request, object target, string methodName,
            params object[] paremeters) =>
            request.CreateSubRequest(request.DesiredType, paremeters).Run(target, methodName);
        
        public static object? Run(this IBindingRequest request, object target, string methodName) => 
            RunStaticOrMemberMethod(request, target, methodName, target.GetType(), MemberMethodSearchCriteria);

        private static object? RunStaticOrMemberMethod(IBindingRequest request, object? target, string methodName, 
            Type targetType, BindingFlags searchCriteria)
        {
            var methods = FindCallableTargetMethod(request, methodName, searchCriteria, targetType);
            return methods.Invoke(target, ResolveParameters(request, methods));
        }

        private static object?[] ResolveParameters(IBindingRequest request, MethodInfo methods) => 
            request.IocService.Get(BindingRequestsFor(request, methods));

        private static List<IBindingRequest> BindingRequestsFor(IBindingRequest request, MethodInfo methods) => 
            methods.GetParameters().Select(request.CreateSubRequest).ToList();

        private static MethodInfo FindCallableTargetMethod(IBindingRequest request, string methodName, 
            BindingFlags searchCriteria, Type typeToSearch) =>
            typeToSearch
                .GetMethods(searchCriteria)
                .Where(i => i.Name.Equals(methodName, StringComparison.Ordinal))
                .OrderByDescending(i => i.GetParameters().Length)
                .FirstOrDefault(i => CanSupplyAllParameters(request, i))
            ??throw new IocException("Could not find a method to call");

        private const BindingFlags MemberMethodSearchCriteria = 
            BindingFlags.Instance|
            BindingFlags.Public|
            BindingFlags.NonPublic|
            BindingFlags.FlattenHierarchy | 
            BindingFlags.InvokeMethod;
        private const BindingFlags StaticMethodSearchCriteria = 
            BindingFlags.Static|
            BindingFlags.Public|
            BindingFlags.NonPublic|
            BindingFlags.FlattenHierarchy | 
            BindingFlags.InvokeMethod;

        private static bool CanSupplyAllParameters(IBindingRequest request, MethodInfo method) =>
            method.GetParameters().All(j => CanSupplyParameter(request, j));

        private static bool CanSupplyParameter(IBindingRequest request, ParameterInfo parameter) => 
            request.IocService.CanGet(request.CreateSubRequest(parameter).Clone());
    }
}