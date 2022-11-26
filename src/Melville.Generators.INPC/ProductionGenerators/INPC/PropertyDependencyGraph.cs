using System;
using System.Collections.Generic;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

public readonly struct PropertyDependencyGraph
{
    private readonly Dictionary<string, HashSet<string>> isReferencedBy;

    public PropertyDependencyGraph(Dictionary<string, HashSet<string>> isReferencedBy)
    {
        this.isReferencedBy = isReferencedBy;
    }

    public IEnumerable<string> AllDependantProperties(string initialProperty) => 
        new TransitiveClosureComputer<string>(LookupDependency, initialProperty).Result;

    private IEnumerable<string> LookupDependency(string i) => 
        isReferencedBy.TryGetValue(i, out var result) ? result : Array.Empty<string>();

}