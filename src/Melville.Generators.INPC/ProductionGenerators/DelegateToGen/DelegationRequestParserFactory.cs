using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public static class DelegationRequestParserFactory
{
    public static DelegationRequestParser Create(ImmutableArray<AttributeData> attrs, SemanticModel compilation) => 
        new(UseExplicit(attrs), StringArgument(attrs), compilation);


    public static bool UseExplicit(ImmutableArray<AttributeData> attrs) =>
        attrs.Any(HasTrueArgument);
    private static bool HasTrueArgument(AttributeData arg) =>
        arg.AllValues().Any(i=>true.Equals(i.Value) );
    private static string StringArgument(ImmutableArray<AttributeData> attrs) =>
        attrs
            .SelectMany(i => i.AllValues())
            .Where(i => i.Type is {SpecialType:SpecialType.System_String})
            .Select(i => i.Value?.ToString())
            .FirstOrDefault() 
            ??"";
}