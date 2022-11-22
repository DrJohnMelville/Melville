using System;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.GenerationTools.AbstractGenerators;
[Obsolete]
public class TestGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(
                "Melville.INPC.HasAttrAttribute", Filter, XformToModel),
            Generate);
    }

    private bool Filter(SyntaxNode arg1, CancellationToken arg2)
    {
        return true;
    }


    private string XformToModel(GeneratorAttributeSyntaxContext context, CancellationToken arg2)
    {
        return context.TargetNode.ToString();
    }
    private void Generate(SourceProductionContext arg1, string arg2)
    {
        arg1.AddSource("out.cs", "//"+arg2);
    }
}