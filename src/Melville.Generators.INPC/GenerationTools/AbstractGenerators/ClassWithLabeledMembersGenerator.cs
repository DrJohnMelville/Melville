using System.IO;
using System.Linq;
using System.Threading;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.GenerationTools.AbstractGenerators;

public interface ILabeledMembersSyntaxModel
{
    public void AddMember(SyntaxNode declSyntax);
    public ILabeledMembersSemanticModel AddSemanticInfo(SemanticModel semanticModel);
}
public interface ILabeledMembersSemanticModel
{
    public TypeDeclarationSyntax ClassDeclaration { get; }
    void GenerateCode(CodeWriter cw);
}

public abstract class ClassWithLabeledMembersGenerator: IIncrementalGenerator
{
    private readonly SearchForAttribute attrFinder;

    protected ClassWithLabeledMembersGenerator(SearchForAttribute attrFinder)
    {
        this.attrFinder = attrFinder;
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.CreateSyntaxProvider(
                    TokenSelector,
                    static (gsc, _) => gsc
                )
                .Collect()
                .SelectMany( (i, _) => i
                    .GroupBy(ParentClassSelector)
                    .Select(GatherClassFields))
            , GenerateClass);
    }

    private  ILabeledMembersSemanticModel GatherClassFields(IGrouping<TypeDeclarationSyntax, GeneratorSyntaxContext> i)
    {
        var ret = CreateMemberRecord(i.Key);
        foreach (var member in i)
        {
            ret.AddMember(member.Node);
        }

        return ret.AddSemanticInfo(i.First().SemanticModel);
    }

    protected abstract ILabeledMembersSyntaxModel CreateMemberRecord(TypeDeclarationSyntax targetTypeDecl);

    private void GenerateClass(SourceProductionContext context, ILabeledMembersSemanticModel classToImplement)
    {
        var cw = new SourceProductionCodeWriter(context);
        classToImplement.GenerateCode(cw);
        cw.PublishCodeInFile(classToImplement.ClassDeclaration, "INPC");
    }

    private static TypeDeclarationSyntax ParentClassSelector(GeneratorSyntaxContext j) =>
        j.Node as TypeDeclarationSyntax ??
        j.Node.Parent as TypeDeclarationSyntax ??
        throw new InvalidDataException("target must be a type of a member of a type;");

    private bool TokenSelector(SyntaxNode sn, CancellationToken _) =>
        sn is MemberDeclarationSyntax mds && attrFinder.HasAttribute(mds);    
}