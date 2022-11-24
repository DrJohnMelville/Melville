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
                static (i, _) => i is MemberDeclarationSyntax,
                static (i, _) => DelegationRequestParser.ParseItem(i.SemanticModel, i.TargetNode)
            ),
            Generate
        );
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName("Melville.INPC.DelegateToAttribute",
                static (i, _) => i is VariableDeclaratorSyntax,
                static (i, _) =>
                #warning -- this is ugly and needs to be fixed
                    DelegatedMethodGenerator.Create(((IFieldSymbol)i.TargetSymbol).Type, 
                        $"this.{i.TargetSymbol.Name}", ClimbTree(i.TargetNode), 
                        (ITypeSymbol)i.SemanticModel.GetDeclaredSymbol(ClimbTree(i.TargetNode).Parent))
                ),
            Generate
        );
    }

    private void Generate(SourceProductionContext writeTo, IDelegatedMethodGenerator? factory)
    {
        if (factory is null) return;
        var cw = new SourceProductionCodeWriter(writeTo);
        factory.GenerateForwardingMethods(cw);
    }

    private static MemberDeclarationSyntax ClimbTree(SyntaxNode node) =>
        node as MemberDeclarationSyntax ?? ClimbTree(node.Parent ??
                                                     throw new InvalidOperationException("Cannot find member declaration."));
}