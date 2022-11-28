using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

[Generator]
public class INPCGenerator: IIncrementalGenerator
{
    private const string attributeFullName = "Melville.INPC.AutoNotifyAttribute";
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(attributeFullName,
                (i,_)=> i is TypeDeclarationSyntax or VariableDeclaratorSyntax,
                (i,_)=> i)
                .Collect()
                .SelectMany((i,_) => 
                    i.GroupBy(j=>j.TargetSymbol.ThisOrContainingTypeSymbol(), SymbolEqualityComparer.Default ))
                .Select((j,_)=>j),
            Generate
        );
    }

    private void Generate(SourceProductionContext context, IGrouping<ISymbol?, GeneratorAttributeSyntaxContext> classToGenerate)
    {
        if (classToGenerate.Key is not ITypeSymbol classSymbol) return;
        var cw = new SourceProductionCodeWriter(context);
        var notifyImplementation = NotifyImplementationStrategyFactory.StrategyForClass(classSymbol);
        var dependencies = new PropertyDependencyGraphFactory().CreateFromClass(classSymbol);
        using (cw.GenerateInClassFile(classSymbol, "INPC", notifyImplementation.DeclareInterface()))
        {
            notifyImplementation.DeclareMethod(cw);
            foreach (var member in classToGenerate)
            {
                new AttributeCopier(cw, "property").CopyAttributesFrom(member.TargetNode);
                if (member.TargetSymbol is not IFieldSymbol field) continue;
                new PropertyGenerator(
                    cw, field, classSymbol, notifyImplementation,dependencies,
                    new PropertyParametersParser(member.Attributes).Parse())
                    .RenderSingleField();
            }
        }
    }
}

;