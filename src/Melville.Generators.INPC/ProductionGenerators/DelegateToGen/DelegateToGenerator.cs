using System;
using System.Collections.Immutable;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.ClassGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

[Generator]
public class DelegateToGenerator: IIncrementalGenerator
{
    private const string QualifiedAttributeName = "Melville.INPC.DelegateToAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        RegisterGeneratorFor<PropertyDeclarationSyntax>(context,
            (item, fact)=> fact.ParseFromProperty((IPropertySymbol)item.TargetSymbol));
        RegisterGeneratorFor<MethodDeclarationSyntax>(context,
            (item, fact)=> fact.ParseFromMethod((IMethodSymbol)item.TargetSymbol, item.TargetNode));
        RegisterGeneratorFor<VariableDeclaratorSyntax>(context,
            (item, fact)=> fact.ParseFromField((IFieldSymbol)item.TargetSymbol));
        RegisterGeneratorFor<TypeDeclarationSyntax>(context,
            (item, fact)=> fact.ParseFromType((ITypeSymbol)item.TargetSymbol));
    }

    private void RegisterGeneratorFor<T>(
        IncrementalGeneratorInitializationContext context,
        Func<GeneratorAttributeSyntaxContext, DelegationRequestParser, IDelegatedMethodGenerator> func) =>
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(QualifiedAttributeName,
                static (i, _) => i is T,
                 (i,_) => (i, func)),
            Generate
        );

    private void Generate(
        SourceProductionContext arg1, 
        (GeneratorAttributeSyntaxContext i, Func<GeneratorAttributeSyntaxContext, DelegationRequestParser, IDelegatedMethodGenerator> func) context)
    {
        var targetAttributes = context.i;
        var codeWriter = new SourceProductionCodeWriter(arg1);
        using var _ = codeWriter.GenerateInClassFile(targetAttributes.TargetNode, "GeneratedDelegator");
        foreach (var attr in targetAttributes.Attributes)
        {
            var creator = DelegationRequestParserFactory.Create(attr, targetAttributes.SemanticModel);
            context.func(context.i,creator).GenerateForwardingMethods(codeWriter);
        }
    }

}
