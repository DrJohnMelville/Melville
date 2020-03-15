using System;
using System.Collections.Generic;
using System.Linq;

namespace Melville.IOC.IocContainers
{
    public class IocException : Exception
    {
        public  List<string> TypeStack { get; } = new List<string>();
        public IocException(string? message) : base(message)
        {
        }

        public IocException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public IocException CreateElaboratedException() => new IocException(CreateMessage(), this);
        private string CreateMessage() => string.Join("\r\n    ", TypesRequested().Prepend(Message));
        private IEnumerable<string> TypesRequested() => 
            TypeStack.Select((typename, index) => $"{index + 1}. {typename}");
    }

    /// <summary>
    /// This is a error handling idiom that lets me print activation stack for all IOC injection errors without
    /// the allocations to maintain a stack prospectively in the usual case where no error occurs.  Basically
    /// if I have an exception, I catch it at every level of Get request and append the name to a list that
    /// I carry around in the exception.  The outer method prints the exception nicely with the stack in the
    /// message string
    /// </summary>

    public static class RecursiveExceptionTracker
    {
        public static object? BasisCall(Func<IBindingRequest, object?> action, IBindingRequest request)
        {
            try
            {
                return action(request);
            }
            catch (IocException e)
            {
                throw e.CreateElaboratedException();
            }
        }
        public static object? RecursiveCall(Func<IBindingRequest, object?> action, IBindingRequest request)
        {
            try
            {
                return action(request);
            }
            catch (IocException e)
            {
                e.TypeStack.Add($"{RequestedTypeName(request)} -- {ScopeTag(request)}");
                throw;
            }
        }
        
        private static string ScopeTag(IBindingRequest request) => request.IocService.IsGlobalScope ? "No Scope" : "In Scope";
        private static string RequestedTypeName(IBindingRequest requestedType)
        {
            return requestedType.DesiredType.FullName??requestedType.DesiredType.Name;
        }

    }
}