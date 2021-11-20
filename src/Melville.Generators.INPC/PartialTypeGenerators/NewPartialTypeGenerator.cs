using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.PartialTypeGenerators;

public abstract class NewPartialTypeGenerator<T>: IIncrementalGenerator
{
    protected abstract T PreProcess(IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input,
        GeneratorExecutionContext context);
    protected abstract bool GlobalDeclarations(CodeWriter cw);
    protected abstract bool GenerateClassContents(T input, CodeWriter cw);
    protected virtual string ClassSuffix(T input) => "";

    private readonly SearchForAttribute[] targetAttributes;
    private readonly string suffix;

    protected NewPartialTypeGenerator(string suffix, params string[] targetAttributes)
    {
        this.targetAttributes = targetAttributes.Select(i=>new SearchForAttribute(i)).ToArray();
        this.suffix = suffix;
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(GenerateStaticOutput);
        var items = context.SyntaxProvider.CreateSyntaxProvider(
                HasAttributes, static (gsc, _) =>
                    (Node: (MemberDeclarationSyntax)gsc.Node, Context:gsc))
            .Where(i => i.Node.Parent != null && targetAttributes.Any(j => j.HasAttribute(i.Node)))
            .Collect()
            .SelectMany((ia, _) => ia.GroupBy(j => j.Node.Parent));
        
        context.RegisterSourceOutput(items, (a,b)=>
        {
            var text = $"//{b}";
            a.AddSource($"Hello{num++}.cs", text);
        });
    }

    private IEnumerable<TResult> SelectMany<TResult>(
        ImmutableArray<(MemberDeclarationSyntax Node, SemanticModel SemanticModel)> arg1, 
        CancellationToken arg2)
    {
        throw new NotImplementedException();
    }

    private int num = 0;
    private bool HasAttributes(SyntaxNode member, CancellationToken arg2)
    {
        return member is MemberDeclarationSyntax mds && mds.AttributeLists.Count > 0;
    }

    private void GenerateStaticOutput(IncrementalGeneratorPostInitializationContext context)
    {
        var cw = new PostInitializationCodeWriter(context);
        if (InnerGlobalDeclarations(cw))
        {
            cw.PublishCodeInFile(CreateFileName($"{suffix}Attributes"));            
        }
    }
    private bool InnerGlobalDeclarations(CodeWriter cw)
    {
        cw.AppendLine($"namespace Melville.{suffix}");
        using var ns = cw.CurlyBlock();
        return GlobalDeclarations(cw);
    }

    private string CreateFileName(string className) => 
        $"{className}.{suffix}.cs";
}

