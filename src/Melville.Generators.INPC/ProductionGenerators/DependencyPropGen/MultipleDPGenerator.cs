using System.Collections.Immutable;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DependencyPropGen;

public readonly struct GenerateMultipleDependencyPropertes
{
    public static readonly SearchForAttribute Searcher = new("Melville.INPC.GenerateDPAttribute");

    private readonly CodeWriter codeWriter;
    private readonly SemanticModel semanticModel;
    private readonly MemberDeclarationSyntax target;
    private readonly ITypeSymbol parentSymbol;

    public GenerateMultipleDependencyPropertes(
        CodeWriter codeWriter, SemanticModel semanticModel, MemberDeclarationSyntax target, 
        ITypeSymbol parentSymbol)
    {
        this.codeWriter = codeWriter;
        this.semanticModel = semanticModel;
        this.target = target;
        this.parentSymbol = parentSymbol;
    }

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
            parser.Generate(codeWriter);
        }
    }

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
