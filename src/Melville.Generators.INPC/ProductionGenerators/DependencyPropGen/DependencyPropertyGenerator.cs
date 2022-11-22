using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DependencyPropGen;
[Generator]
public class DependencyPropertyGenerator: IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(
                "Melville.INPC.GenerateDPAttribute", 
                static (i,_)=>i is MemberDeclarationSyntax or VariableDeclaratorSyntax,
                Factory)
            , Generate);
    }

    private record DpRootItem(
        MemberDeclarationSyntax Target, ITypeSymbol? Parent, SemanticModel SemanticModel);
    
    private DpRootItem Factory(
        GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        var targetNote = ClimbTree(context.TargetNode)!;
        return new DpRootItem(
            targetNote,
            ParentSymbol(context, context.TargetNode),
            context.SemanticModel);
    }

    private MemberDeclarationSyntax ClimbTree(SyntaxNode node) =>
        node as MemberDeclarationSyntax ?? ClimbTree(node.Parent ??
                  throw new InvalidOperationException("Cannot find member declaration."));

    private static ITypeSymbol? ParentSymbol(GeneratorAttributeSyntaxContext context, SyntaxNode node) =>
        ParentDeclaration(node) is { } parentNode
            ? context.SemanticModel.GetDeclaredSymbol(parentNode) as ITypeSymbol
            : null;
    private static TypeDeclarationSyntax? ParentDeclaration(SyntaxNode member) => 
        member.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().FirstOrDefault();

    
    private void Generate(SourceProductionContext context, DpRootItem item)
    {
        var cw = new SourceProductionCodeWriter(context);
        if (item.Parent is null)
        {
            ReportNoParentSymbol(cw, item.Target);
            return;
        }

        using (cw.GenerateInClassFile(item.Target, "DepPropGen"))
        {
            new GenerateMultipleDependencyPropertes(cw, item.SemanticModel, item.Target, item.Parent)
                .Generate();
        }
    }
    private static void ReportNoParentSymbol(CodeWriter cw, SyntaxNode targetMember) =>
        cw.ReportDiagnostic(Diagnostic.Create(
            new DiagnosticDescriptor("DPGen0001", "Cannot find symbol info",
                $"No symbol info for {targetMember}", "Generation",
                DiagnosticSeverity.Error, true),
            Location.Create(targetMember.SyntaxTree, targetMember.Span)));

    /*    public DependencyPropertyGenerator() : 
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
            */
}