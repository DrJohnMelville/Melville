using System.Linq;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DependencyPropGen;
[Generator]
public class DependencyPropertyGenerator: LabeledMemberGenerator
{
    public DependencyPropertyGenerator() : 
        base(GenerateMultipleDependencyPropertes.Searcher, "DepPropGen")
    {
    }

    protected override bool GenerateCodeForMember(GeneratorSyntaxContext member, CodeWriter cw)
    {
        var targetMember = (MemberDeclarationSyntax)member.Node;

        if (ParentDeclaration(member) is not {} parentSyntax ||
            member.SemanticModel.GetDeclaredSymbol(parentSyntax) is not ITypeSymbol classSymbol)
        {
            ReportNoParentSymbol(cw, targetMember);
            return false;
        }

        new GenerateMultipleDependencyPropertes(cw, member.SemanticModel,
            targetMember, classSymbol).Generate();

        return true;
    }

    private static TypeDeclarationSyntax? ParentDeclaration(GeneratorSyntaxContext member) => 
        member.Node.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().FirstOrDefault();

    private static void ReportNoParentSymbol(CodeWriter cw, MemberDeclarationSyntax targetMember) =>
        cw.ReportDiagnostic(Diagnostic.Create(
            new DiagnosticDescriptor("DPGen0001", "Cannot find symbol info",
                $"No symbol info for {targetMember}", "Generation",
                DiagnosticSeverity.Error, true),
            Location.Create(targetMember.SyntaxTree, targetMember.Span)));
}