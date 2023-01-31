using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public static class DelegationRequestParserFactory
{
    public static DelegationRequestParser Create(ImmutableArray<AttributeData> attrs) => new(UseExplicit(attrs));
    
    public static bool UseExplicit(ImmutableArray<AttributeData> attrs) =>
        attrs.Any(HasTrueArgument);
    private static bool HasTrueArgument(AttributeData arg) =>
        arg.AllValues().Any(i=>true.Equals(i.Value) );
}