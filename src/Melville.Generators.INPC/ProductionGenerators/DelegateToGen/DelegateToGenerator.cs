using System;
using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

[Generator]
public class DelegateToGenerator: IIncrementalGenerator
{
    public const string QualifiedAttributeName = "Melville.INPC.DelegateToAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        RegisterGeneratorFor<PropertyDeclarationSyntax>(context,
            (item, fact)=> fact.ParseFromProperty((IPropertySymbol)item.TargetSymbol));
        RegisterGeneratorFor<MethodDeclarationSyntax>(context,
            (item, fact)=> fact.ParseFromMethod((IMethodSymbol)item.TargetSymbol, item.TargetNode));
        RegisterGeneratorFor<VariableDeclaratorSyntax>(context,
            (item, fact)=> fact.ParseFromField((IFieldSymbol)item.TargetSymbol));
    }

    private void RegisterGeneratorFor<T>(
        IncrementalGeneratorInitializationContext context,
        Func<GeneratorAttributeSyntaxContext, DelegationRequestParser, IDelegatedMethodGenerator> func)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(QualifiedAttributeName,
                static (i, _) => i is T,
                (i, _) => new DelegatedMethodInLocation(i.TargetNode,
                             func(i, CreateRequestParser(i.Attributes)))),
            Generate
        );
        
    }

    private static DelegationRequestParser CreateRequestParser(ImmutableArray<AttributeData> attributes)
    {
        return new DelegationRequestParser(DelegateToArgumentParser.UseExplicit(attributes));
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