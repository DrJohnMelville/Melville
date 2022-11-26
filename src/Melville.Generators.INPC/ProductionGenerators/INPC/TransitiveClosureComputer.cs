using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

public readonly struct TransitiveClosureComputer<T>
{
    public HashSet<T> Result { get; }= new();
    private readonly Func<T, IEnumerable<T>> relation;

    public TransitiveClosureComputer(Func<T, IEnumerable<T>> relation, T seed)
    {
        this.relation = relation;
        ProcessCandidate(seed);
    }

    private void ProcessCandidate(T item)
    {
        if (Result.Contains(item)) return;
        Result.Add(item);
        AddCandidates(relation(item));
    }

    private void AddCandidates(IEnumerable<T> newItems)
    {
        foreach (var item in newItems)
        {
            ProcessCandidate(item);
        }
    }
}