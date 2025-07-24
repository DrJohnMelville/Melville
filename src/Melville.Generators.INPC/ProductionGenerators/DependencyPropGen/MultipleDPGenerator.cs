using System;
using System.Collections.Immutable;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DependencyPropGen;

public enum FrameworkType
{
    Wpf = 0,
    Maui = 1
}; 

public readonly struct GenerateMultipleDependencyPropertes(
    CodeWriter codeWriter,
    SemanticModel semanticModel,
    MemberDeclarationSyntax target,
    ITypeSymbol parentSymbol,
    FrameworkType frameworkType)
{
    public static readonly SearchForAttribute DpSearcher = 
        new("Melville.INPC.GenerateDPAttribute");
    public static readonly SearchForAttribute BpSearcher = 
        new("Melville.INPC.GenerateBPAttribute");

    private SearchForAttribute Searcher => frameworkType switch
    {
        FrameworkType.Wpf => DpSearcher,
        FrameworkType.Maui => BpSearcher,
        _ => throw new ArgumentOutOfRangeException(nameof(frameworkType))
    };

    public void Generate()
    {
        foreach (var attribute in Searcher.FindAllAttributes(target))
        {
            GenerateSingleDependencyProperty(attribute);
        }
    }

    private void GenerateSingleDependencyProperty(AttributeSyntax attribute)
    {
        if (ParseAttribute(attribute) is { } parser &&
            EnsureValidAttribute(attribute, parser))
        {
            CreateDependencyPropertyGenerator(parser)
                .GenerateDependencyPropertyDeclaration();
        }
    }

    private AbstractPropertyCodeGenerator CreateDependencyPropertyGenerator(RequestParser parser) =>
        frameworkType switch
        {
            FrameworkType.Maui => new BindablePropertyCodeGenerator(parser, codeWriter),
            _ => new DependencyPropertyCodeGenerator(parser, codeWriter)
        };

    private RequestParser ParseAttribute(AttributeSyntax attribute)
    {
        var parser = new RequestParser(semanticModel, parentSymbol, target);
        // we parse the target before the parameters, because the parameters override the conventions.
        parser.ParseAllParams(attribute.ArgumentList);
        return parser;
    }

    private bool EnsureValidAttribute(AttributeSyntax attribute, RequestParser parser)
    {
        if (parser.Valid()) return true;
        codeWriter.ReportDiagnosticAt(attribute, "DPGen002", "Cannot resolve dependency property.",
            $"{attribute} does not have enough information to create DP.",
            DiagnosticSeverity.Error);
        return false;
    }
}
