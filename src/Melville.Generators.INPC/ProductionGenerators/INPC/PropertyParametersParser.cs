using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

public readonly struct PropertyParametersParser
{
    private readonly ImmutableArray<AttributeData> attributes;

    public PropertyParametersParser(ImmutableArray<AttributeData> attributes)
    {
        this.attributes = attributes;
    }

    public string Parse()
    {
        return attributes
            .SelectMany(i => i.NamedArguments)
            .Where(i => i.Key.Equals("PropertyModifier", StringComparison.Ordinal))
            .Select(i => i.Value.Value?.ToString()??"public")
            .DefaultIfEmpty("public")
            .First();
    }
}