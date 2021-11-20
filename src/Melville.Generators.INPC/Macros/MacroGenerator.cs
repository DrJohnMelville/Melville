using System.Linq;
using System.Text;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.Macros;

[Generator]
public class MacroGenerator : IIncrementalGenerator
{
    private static readonly SearchForAttribute attrFinder = new("Melville.INPC.MacroCodeAttribute");
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.CreateSyntaxProvider(
                static (sn, _) => sn is MemberDeclarationSyntax mds && attrFinder.HasAttribute(mds),
                static (gsc, _) => (MemberDeclarationSyntax)gsc.Node
            ), GenerateCode);
    }

    private void GenerateCode(SourceProductionContext context, MemberDeclarationSyntax member)
    {
        var cw = new SourceProductionCodeWriter(context);
        using (WriteCodeNear.Symbol(member, cw))
        {
            MacroSyntaxInterpreter.ExpandSingleMacroSet(member, cw);
        }
        cw.PublishCodeInFile(FileNameForMember(member, "MacroGen"));
    }

    private string FileNameForMember(MemberDeclarationSyntax member, string postfix)
    {
        var sb = new StringBuilder();
        foreach (var symbol in member.AncestorsAndSelf().Reverse())
        {
            sb.Append(NameForNode(symbol, postfix));
            sb.Append('.');
        }
        sb.Append("cs");
        return sb.ToString();
    }

    private string NameForNode(SyntaxNode symbol, string postfix) => symbol switch
    {
        MethodDeclarationSyntax mds => mds.Identifier.ToString(),
        PropertyDeclarationSyntax pds => pds.Identifier.ToString(),
        EventDeclarationSyntax pds => pds.Identifier.ToString(),
        IndexerDeclarationSyntax pds => "Indexer",
        FieldDeclarationSyntax fds => string.Join(
            "", fds.Declaration.Variables.Select(i => i.Identifier.ToString())),
        BaseTypeDeclarationSyntax btds => btds.Identifier.ToString(),
        BaseNamespaceDeclarationSyntax nds => nds.Name.ToString(),
        CompilationUnitSyntax => postfix,
        _ => "Unnamed"
    };

}