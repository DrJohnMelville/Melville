using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Melville.Generators.INPC.PartialTypeGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.DependencyPropGen;

public record DpGenerationRequest(
    ITypeSymbol TypeSymbol,
    SemanticModel SemanticModel,
    IEnumerable<MemberDeclarationSyntax> Members);

[Generator]
public class DependencyPropertyGenerator: PartialTypeGenerator
{
    public DependencyPropertyGenerator() : 
        base("DependencyPropertyGeneration",
            "Melville.INPC.GenerateDPAttribute")
    {
    }

    protected override bool GlobalDeclarations(CodeWriter cw)
    {
        return false;
    }

    protected override bool 
        GenerateClassContents(IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input, 
            CodeWriter cw)
    {
        var semanticModel = cw.Context.Compilation.GetSemanticModel(input.Key.SyntaxTree);
        var symbolInfo = semanticModel.GetDeclaredSymbol(input.Key);
        if (!(symbolInfo is ITypeSymbol classSymbol))
        {
            cw.ReportDiagnosticAt(input.Key, "DPGen0001", "Cannot find symbol info",
                $"No symbol info for {input.Key.Identifier}", DiagnosticSeverity.Error);
            return false;
        }

        var req = new DpGenerationRequest(classSymbol, semanticModel, input);
        GenerateAttributes(req, cw);

        return true;
    }

    private static readonly SearchForAttribute searcher = new("Melville.INPC.GenerateDPAttribute"); 
    private static void GenerateAttributes(DpGenerationRequest request, CodeWriter cw)
    {
        foreach (var node in request.Members)
        {
            foreach (var attribute in searcher.FindAllAttributes(node))
            {
                GenerateSingleDependencyProperty(cw, attribute, node, request);
            }
        }
    }

    private static void GenerateSingleDependencyProperty(CodeWriter cw, AttributeSyntax attribute,
        MemberDeclarationSyntax targetMember, DpGenerationRequest request)
    { 
        if (ParseAttribute(attribute, request, targetMember) is { } parser &&
            EnsureValidAttribute(cw, attribute, parser))
        {
            parser.Generate(cw);
        }
    }

    private static bool EnsureValidAttribute(CodeWriter cw, AttributeSyntax attribute, RequestParser parser)
    {
        if (parser.Valid()) return true;
        cw.ReportDiagnosticAt(attribute, "DPGen002", "Cannot resolve dependency property.",
            $"{attribute} does not have enough information to create DP.",
            DiagnosticSeverity.Error);
        return false;
    }

    private static RequestParser ParseAttribute(AttributeSyntax attribute, DpGenerationRequest request, MemberDeclarationSyntax targetMember)
    {
        var parser = new RequestParser(request.SemanticModel, request.TypeSymbol);
        parser.ParseAttributeTarget(targetMember);
        // we parse the target before the parameters, because the parameters override the conventions.
        parser.ParseAllParams(attribute.ArgumentList);
        return parser;
    }
}