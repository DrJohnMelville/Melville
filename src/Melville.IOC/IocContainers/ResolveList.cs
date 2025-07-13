using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using Melville.INPC;
using Melville.IOC.BindingRequests;
using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers;

public ref partial struct ParameterMatcher
{
    [FromConstructor] private readonly IIocService service;
    [FromConstructor] private readonly IEnumerable<IBindingRequest> requests;
    [FromConstructor] private readonly Span<object?> destination;
    [FromConstructor ] private Span<int> paramUsed;

    public void Fill()
    {
        if (destination.Length == 0) return;
        paramUsed.Fill(-1);
        foreach (var (pos, request) in requests.Index())
        {
            if (pos >= destination.Length) return;
            FillSingle(pos, request);
            if (request.IsCancelled)
            {
                CleanUp();
                return;
            }
        }
    }

    private void CleanUp()
    {
        foreach (var dest in destination)
        {
            (dest as IDisposable)?.Dispose();
        }
    }
#warning lots of duplication in this class needs to be cleaned up.
    private void FillSingle(int pos, IBindingRequest request)
    {
        foreach (var (position, value) in request.ArgumentsFromParent.Index())
        {
            if (ObjectFillsRequest(value, request))
            {
                if (paramUsed.IndexOf(position) is >= 0 and var priorPosition &&
                    Object.Equals(value, destination[priorPosition])) continue;

                destination[pos] = value;
                paramUsed[pos] = position;
                return;
            }
        }
        destination[pos] = service.Get(request);
    }
    private static bool ObjectFillsRequest(object value, IBindingRequest request) =>
        request.DesiredType.IsInstanceOfType(value) ||
        (value is ReplaceLiteralArgumentWithNonsenseValue fake && fake.CanAssignTo(request.DesiredType));

    public bool CanFill()
    {
        if (destination.Length == 0) return true;
        paramUsed.Fill(-1);
        foreach (var (pos, request) in requests.Index())
        {
            if (pos >= destination.Length) return true;
            if (!CanFillSingle(pos, request)) return false;
        }
        return true;
    }

    public bool CanFillSingle(int pos, IBindingRequest request)
    {
        foreach (var (position, value) in request.ArgumentsFromParent.Index())
        {
            if (ObjectFillsRequest(value, request))
            {
                if (paramUsed.IndexOf(position) is >= 0 and var priorPosition &&
                    Object.Equals(value, destination[priorPosition])) continue;
                return true;
            }
        }
        return service.CanGet(request);
    }

}

public static class ResolveList
{
    public static void Fill(
        this IIocService service, Span<object?> destination, IEnumerable<IBindingRequest> requests) =>
        new ParameterMatcher(service, requests, destination, stackalloc int[destination.Length]).Fill();

    public static bool CanGet(this IIocService ioc, IEnumerable<IBindingRequest> requests)
    {
        var requestCol = requests as IReadOnlyCollection<IBindingRequest> ?? requests.ToList();
        object?[] buffer = ArrayPool<object?>.Shared.Rent(requestCol.Count);
        try
        {
            return new ParameterMatcher(ioc, requestCol, buffer.AsSpan(0, requestCol.Count),
                stackalloc int[requestCol.Count]).CanFill();
        }
        finally
        {
            ArrayPool<object?>.Shared.Return(buffer);
        }
    }
}