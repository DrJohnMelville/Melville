using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public static class DelegationRequestParserFactory
{
    public static DelegationRequestParser Create(AttributeData attr, SemanticModel compilation)
    {
        bool useExplicit = false;
        string WrapWith = "";
        foreach (var ca in attr.ConstructorArguments)
        {
            if (true.Equals(ca.Value)) useExplicit = true;
        }

        foreach (var na in attr.NamedArguments)
        {
            switch (na.Key)
            {
                case "WrapWith": WrapWith = na.Value.Value.ToString(); break;
            }
        }
        return new(useExplicit, WrapWith, compilation);
    }

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