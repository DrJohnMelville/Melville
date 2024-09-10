using System;
using System.Linq;
using System.Threading;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DependencyPropGen;
[Generator]
public class DependencyPropertyGenerator: IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        RegisterPropertySeeker(context, "Melville.INPC.GenerateDPAttribute", Generate);
        RegisterPropertySeeker(context, "Melville.INPC.GenerateBPAttribute", GenerateMaui);
    }

    private void RegisterPropertySeeker(
        IncrementalGeneratorInitializationContext context, 
        string attrName, Action<SourceProductionContext, DpRootItem> generate)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(
                attrName, 
                static (i,_)=>i is MemberDeclarationSyntax or VariableDeclaratorSyntax,
                Factory)
            , generate );
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

    
    private void Generate(SourceProductionContext context, DpRootItem item) => 
        InnerGenerate(context, item, FrameworkType.Wpf);
    private void GenerateMaui(SourceProductionContext context, DpRootItem item) => 
        InnerGenerate(context, item, FrameworkType.Maui);

    private static void InnerGenerate(
        SourceProductionContext context, DpRootItem item, FrameworkType ft)
    {
        var cw = new SourceProductionCodeWriter(context);
        if (item.Parent is null)
        {
            ReportNoParentSymbol(cw, item.Target);
            return;
        }

        using (cw.GenerateInClassFile(item.Target, "DepPropGen"))
        {
            new GenerateMultipleDependencyPropertes(
                    cw, item.SemanticModel, item.Target, item.Parent, ft)
                .Generate();
        }
    }

    private static void ReportNoParentSymbol(CodeWriter cw, SyntaxNode targetMember) =>
        cw.ReportDiagnostic(Diagnostic.Create(
            new DiagnosticDescriptor("DPGen0001", "Cannot find symbol info",
                $"No symbol info for {targetMember}", "Generation",
                DiagnosticSeverity.Error, true),
            Location.Create(targetMember.SyntaxTree, targetMember.Span)));

}