using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Melville.INPC;
using Melville.IOC.BindingRequests;
using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers;

public static class ResolveList
{
    public static void Fill(
        this IIocService service, Span<object?> destination, IEnumerable<IBindingRequest> requests) =>
        new ParameterMatcher(service, requests, destination, stackalloc int[destination.Length]).Fill();

    public static bool CanGet(this IIocService ioc, IEnumerable<IBindingRequest> requests)
    {
        var requestCol = requests as IReadOnlyCollection<IBindingRequest> ?? requests.ToList();
        return CanGet(ioc, requestCol, requestCol.Count);
    }

    public static bool CanGet(IIocService ioc, IEnumerable<IBindingRequest> requestCol, int length)
    {
            return new ParameterMatcher(ioc, requestCol, [], stackalloc int[length]).CanFill();
    }
}

public ref partial struct ParameterMatcher
{
    [FromConstructor] private readonly IIocService service;
    [FromConstructor] private readonly IEnumerable<IBindingRequest> requests;
    [FromConstructor] private readonly Span<object?> destination;
    [FromConstructor ] private Span<int> parametersFilled;

    public void Fill()
    {
        if (!TryFindArguments(request => (request.IocService.Get(request), !request.IsCancelled))) CleanUpAfterFailedCreate();
    }

    public bool CanFill() => TryFindArguments(request => (null, request.IocService.CanGet(request)));

    private bool TryFindArguments(Func<IBindingRequest, (object? value, bool success)> getArgument)
    {
        if (parametersFilled.Length == 0) return true;
        parametersFilled.Fill(-1);
        foreach (var (parameterPosition, request) in requests.Take(parametersFilled.Length).Index())
        {
            if (TryFindArgumentForRequest(request, parameterPosition, out var argument))
            {
                TrySetDestination(parameterPosition, argument);
            }
            else
            {
                var (value, success) = getArgument(request);
                if (!success) return false;
                TrySetDestination(parameterPosition, value);
            }
        }
        return true;
    }

    private void TrySetDestination(int parameterPosition, object? argument)
    {
        if (parameterPosition < destination.Length)
            destination[parameterPosition] = argument;
    }

    private bool TryFindArgumentForRequest(IBindingRequest request, int parameterPosition, out object? argument)
    {
        foreach (var (argumentPosition, arg) in request.Arguments.Index())
        {
            if (ArgumentSatisfiesRequest(arg, argumentPosition, request))
            {
                argument = arg;
                parametersFilled[parameterPosition] = argumentPosition;
                return true;
            }
        }
        argument = null;
        return false;
    }

    private bool ArgumentSatisfiesRequest(object? argument, int argumentPosition, IBindingRequest request) => 
        ObjectFillsRequest(argument, request) && !ParameterHasBeenUsed(argumentPosition);

    private static bool ObjectFillsRequest(object? value, IBindingRequest request) =>
        request.DesiredType.IsInstanceOfType(value) ||
        (value is ReplaceLiteralArgumentWithNonsenseValue fake && fake.CanAssignTo(request.DesiredType));

    private bool ParameterHasBeenUsed(int position) => parametersFilled.IndexOf(position) is >= 0;

    private void CleanUpAfterFailedCreate()
    {
        foreach (var dest in destination)
        {
            (dest as IDisposable)?.Dispose();
        }
    }

}