using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.Macros;

[Generator]
public class MacroGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(
                "Melville.INPC.MacroCodeAttribute", 
                (_,_) => true,
                (i, _) => i),
            Generate);
 
    }

    private void Generate(SourceProductionContext context, GeneratorAttributeSyntaxContext item)
    {
        var cw = new SourceProductionCodeWriter(context);
        using (cw.GenerateInClassFile(item.TargetNode, "MacroGen"))
        {
            new MacroExpander(item.TargetSymbol.GetAttributes()).WriteMacros(cw);
        }
    }
}