using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public static class DelegateToArgumentParser
{
    public static bool UseExplicit(ImmutableArray<AttributeData> attrs) =>
        attrs.Where(i => i.AttributeClass?.FullyQualifiedName()==DelegateToGenerator.QualifiedAttributeName)
            .Any(HasTrueArgument);

    private static bool HasTrueArgument(AttributeData arg) =>
        arg.ConstructorArguments
            .Concat(arg.NamedArguments.Select(i=>i.Value))
            .Any(i=>true.Equals(i.Value) );
}