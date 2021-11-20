using System.Threading;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.PartialTypeGenerators;

public abstract class LabeledMemberGenerator : IIncrementalGenerator
{
    private readonly SearchForAttribute attrFinder;
    private readonly string filePrefix;

    protected LabeledMemberGenerator(SearchForAttribute attrFinder, string filePrefix)
    {
        this.attrFinder = attrFinder;
        this.filePrefix = filePrefix;
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.CreateSyntaxProvider(
                TokenSelector,
                static (gsc, _) => gsc
            ), GenerateCode);
    }

    private bool TokenSelector(SyntaxNode sn, CancellationToken _) => 
        sn is MemberDeclarationSyntax mds && attrFinder.HasAttribute(mds);

    private void GenerateCode(SourceProductionContext context, GeneratorSyntaxContext member)
    {
        var cw = new SourceProductionCodeWriter(context);
        using (WriteCodeNear.Symbol(member.Node, cw))
        {
            if (!GenerateCodeForMember(member, cw)) return;
        }
        cw.PublishCodeInFile(member.Node, filePrefix);
    }

    protected abstract bool GenerateCodeForMember(GeneratorSyntaxContext member, CodeWriter cw);
}