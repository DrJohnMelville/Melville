using System;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

[Generator]
public class DelegateToGenerator: IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName("Melville.INPC.DelegateToAttribute",
                static (i, _) => i is MemberDeclarationSyntax or VariableDeclaratorSyntax,
                static (i, _) =>
                {
                    var targetSyntax = ClimbTree(i.TargetNode);
                    return new DelegatedMethodInLocation(targetSyntax, 
                        DelegationRequestParser.ParseItem(i.SemanticModel, targetSyntax));
                }),
            Generate
        );
    }

    private void Generate(SourceProductionContext writeTo, DelegatedMethodInLocation factory)
    {
        var cw = new SourceProductionCodeWriter(writeTo);
        factory.GenerateIn(cw);
    }

    private static MemberDeclarationSyntax ClimbTree(SyntaxNode node) =>
        node as MemberDeclarationSyntax ?? ClimbTree(node.Parent ??
                                                     throw new InvalidOperationException("Cannot find member declaration."));
}