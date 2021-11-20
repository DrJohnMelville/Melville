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
        cw.PublishCodeInFile(member, "MacroGen");
    }
}