using System.Linq;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.PartialTypeGenerators;

public abstract class NewPartialTypeGenerator<T>: IIncrementalGenerator
{
    protected abstract T PreProcess(
        IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input,
        GeneratorExecutionContext context);
    protected abstract bool GlobalDeclarations(CodeWriter cw);
    protected abstract bool GenerateClassContents(T input, CodeWriter cw);
    protected virtual string ClassSuffix(T input) => "";

    private readonly string[] targetAttributes;
    private readonly string suffix;

    protected NewPartialTypeGenerator(string suffix, string[] targetAttributes)
    {
        this.targetAttributes = targetAttributes;
        this.suffix = suffix;
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(GenerateStaticOutput);
    }

    private void GenerateStaticOutput(IncrementalGeneratorPostInitializationContext obj)
    {
    }
}