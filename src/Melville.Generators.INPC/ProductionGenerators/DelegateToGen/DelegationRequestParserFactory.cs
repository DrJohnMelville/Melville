using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Melville.Generators.INPC.GenerationTools.DocumentationCopiers;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodNamers;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public static class DelegationRequestParserFactory
{
    public static DelegationRequestParser Create(AttributeData attr, SemanticModel compilation,
        IDocumentationLibrary moduleCacheLibrary)
    {
        var useExplicit = false;
        var WrapWith = "";
        var visibility = Accessibility.NotApplicable;
        string? filter = null;
        string? rename = null;
        string? exclude = null;
        foreach (var ca in attr.ConstructorArguments)
        {
            if (true.Equals(ca.Value)) useExplicit = true;
        }

        foreach (var na in attr.NamedArguments)
        {
            switch (na.Key)
            {
                case "WrapWith": WrapWith = na.Value.Value?.ToString()??""; break;
                case "Filter": filter= na.Value.Value?.ToString()??""; break;
                case "Rename": rename = na.Value.Value?.ToString()??""; break;
                case "Exclude": exclude= na.Value.Value?.ToString()??""; break;
                case "ExplicitImplementation": useExplicit = Convert.ToBoolean(na.Value.Value); break;
                case "Visibility": visibility = (Accessibility)(na.Value.Value ?? Accessibility.NotApplicable); break;
            }
        }
        return new(useExplicit, WrapWith, compilation, visibility, 
            RenameFactory.Create(filter, rename, exclude), moduleCacheLibrary);
    }
}