using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.GenerationTools.AstUtilities;

public static class AttributeDataParsers
{
    public static IEnumerable<AttributeData> FilterToAttributeType(
        this IEnumerable<AttributeData> items, string fullyQualifiedName) =>
        items.Where(i => i.AttributeClass?.FullyQualifiedName() == fullyQualifiedName);

    public static IEnumerable<TypedConstant> AllValues(this AttributeData attr) =>
        attr.ConstructorArguments.Concat(attr.NamedArguments.Select(i => i.Value));

}