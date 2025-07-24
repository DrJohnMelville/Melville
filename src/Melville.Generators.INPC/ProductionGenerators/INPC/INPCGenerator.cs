using System.Linq;
using System.Threading;
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
                DesiredSyntaxTypes,
                (i,_)=> i)
                .Collect()
                .SelectMany((i,_) => 
                    i.GroupBy(j=>j.TargetSymbol.ThisOrContainingTypeSymbol(), SymbolEqualityComparer.Default ))
                .Select((j,_)=>j),
            Generate
        );
    }

    private bool DesiredSyntaxTypes(SyntaxNode i, CancellationToken _)
    {
        return i is TypeDeclarationSyntax or VariableDeclaratorSyntax or
            PropertyDeclarationSyntax;
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
                GeneratorFor(
                        cw, member, classSymbol, notifyImplementation, dependencies)
                    ?.RenderSingleField();
            }
        }
    }

    private static PropertyGenerator? GeneratorFor(
        SourceProductionCodeWriter cw, 
        GeneratorAttributeSyntaxContext member, ITypeSymbol classSymbol, 
        INotifyImplementationStategy notifyImplementation, 
        PropertyDependencyGraph dependencies) =>
        member.TargetSymbol switch{
            IFieldSymbol field => new PropertyGenerator(
                cw, new FieldSourceSymbol(field, member),
                classSymbol, notifyImplementation,dependencies),
            IPropertySymbol prop when
                    prop.IsPartialDefinition => new PropertyGenerator(
                cw, new PropertySourceSymbol(prop, member.TargetNode), 
                classSymbol, notifyImplementation, dependencies),
            _=> null
        };
}

;