using System;
using System.Linq;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.PartialTypeGenerators;

public abstract class PartialTypeGenerator<T> : ISourceGenerator
{
    protected abstract bool GlobalDeclarations(CodeWriter cw);
    protected abstract T PreProcess(
        IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input,
        GeneratorExecutionContext context);
    protected abstract bool GenerateClassContents(T input, CodeWriter cw);
    private string ClassSuffix(T input) => "";

    private readonly string[] targetAttributes;
    private readonly string suffix;
    private readonly GeneratedFileUniqueNamer namer;

    protected PartialTypeGenerator(string suffix, params string[] targetAttributes)
    {
        this.suffix = suffix;
        this.targetAttributes = targetAttributes;
        namer = new(suffix);
    }

    public void Initialize(GeneratorInitializationContext context) => 
        context.RegisterForSyntaxNotifications(()=>new PartialTypeReceiver(targetAttributes));

    public void Execute(GeneratorExecutionContext context)
    {
        namer.Clear();
        if (!(context.SyntaxReceiver is PartialTypeReceiver ptr)) return;
        try
        {
            GenerateCodeForAllClasses(context, ptr);
        }
        catch (Exception e)
        {
            ReportExceptionAsCompilerError(context, e);
        }
    }

    private static void ReportExceptionAsCompilerError(GeneratorExecutionContext context, Exception e)
    {
        context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("INPCGen0003",
                "Exception Thrown", e.Message + e.StackTrace, "Fatal", DiagnosticSeverity.Error, true),
            Location.None));
    }

    private void GenerateCodeForAllClasses(GeneratorExecutionContext context, PartialTypeReceiver ptr)
    {
        TryGenerateConstantContent(context);

        foreach (var (group, intermed) in ptr.ItemsByType().Select(i=>(i, PreProcess(i,context))).ToList())
        {
            TryGeneratePartialClass(group.Key, context, intermed);
        }
    }

    private void TryGenerateConstantContent(GeneratorExecutionContext context)
    {
        var codeWriter = new GeneratorContextCodeWriter(context);
        if (InnerGlobalDeclarations(codeWriter))
        {
            codeWriter.PublishCodeInFile(namer.CreateFileName($"{suffix}Attributes"));
        }
    }

    private bool InnerGlobalDeclarations(CodeWriter cw)
    {
        cw.AppendLine($"namespace Melville.{suffix}");
        using var ns = cw.CurlyBlock();
        return GlobalDeclarations(cw);
    }

    private void TryGeneratePartialClass(
        TypeDeclarationSyntax parent, GeneratorExecutionContext context, T input)
    {
        var cw = new GeneratorContextCodeWriter(context);
        using (WriteCodeNear.Symbol(parent, cw))
        {
            if (!GenerateClassContents(input, cw)) return;
        }
        cw.PublishCodeInFile(namer.CreateFileName(parent.Identifier.ToString()));
        
    }
}
    
public abstract class PartialTypeGenerator:
    PartialTypeGenerator<IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax>>
{
    protected PartialTypeGenerator(string suffix, params string[] targetAttributes) : 
        base(suffix, targetAttributes)
    {
    }

    protected override IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> 
        PreProcess(IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input,
            GeneratorExecutionContext context) => input;
}