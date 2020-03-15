using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Melville.IOC
{
    public class MethodResolver
    {
        protected readonly IServiceProvider provider;
        protected MethodResolver(IServiceProvider provider)
        {
            this.provider = provider;
        }

        protected (MethodBase Method, object[]? Arguments) SelectStratecy(IEnumerable<MethodBase> methods, object[] arguments)
        {
            var callStrategy = methods
                .OrderByDescending(i => i.GetParameters().Length)
                .Select<MethodBase, (MethodBase Method, object[]? Arguments)>(i => (Method: i, Arguments: ComputeParameters(i.GetParameters(), arguments)))
                .FirstOrDefault(i => i.Arguments != null);
            return callStrategy;
        }

        private object[]? ComputeParameters(ParameterInfo[] parameters, object[] arguments)
        {
            var ret = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (!TryComputeArgument(parameters[i], arguments, out ret[i])) return null;
            }
            return ret;
        }

        private bool TryComputeArgument(ParameterInfo parameter, object[] arguments, out object o)
        {
            foreach (var arg in arguments)
            {
                if (parameter.ParameterType.IsInstanceOfType(arg))
                {
                    o = arg;
                    return true;
                }
            }

            o = provider.GetService(parameter.ParameterType);
            return o != null;
        }
    }
}