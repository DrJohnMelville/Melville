using System.Linq;
using System.Text.RegularExpressions;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Melville.Generators.INPC.GenerationTools.DocumentationCopiers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

public class PropertySourceSymbol(IPropertySymbol prop, SyntaxNode sourceSyntax): IPropertySourceSymbol
{
    public string FieldName { get; } = $"__generated_{prop.Name}";
    public string PropertyName => prop.Name;
    public ITypeSymbol Type => prop.Type;
    public ISymbol DocumentationSource => prop;

    public SyntaxNode SourceSyntax { get; } = sourceSyntax;

    public string PropertyAccessLevel => (prop.DeclaringSyntaxReferences[0].GetSyntax() as PropertyDeclarationSyntax)?.Modifiers.ToString()
                                         ?? "";
    public void TryWriteFieldDeclaration(CodeWriter target)
    {
        target.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
        new AttributeCopier(target, "field").CopyAttributesFrom(SourceSyntax);
        new DocumentationCopier(target).Copy(DocumentationSource);
        target.AppendLine($"private {Type.FullyQualifiedName()} {FieldName}{TryGetInitalizer()};");
        target.AppendLine();
    }

    private string TryGetInitalizer() => 
        GetInitializedValue() is { } value ? $" = {value}" : "";

    private string? GetInitializedValue() =>
        TrailingComment() is { } trivia &&
        DefaultValueFinder.Match(trivia.ToString()) is {Success:true} match ?
            match.Groups[1].Value: null;

    private static Regex DefaultValueFinder { get; } =
        new Regex(@"^\s*//#\s*=\s*([^;]+);");

    private SyntaxTrivia TrailingComment()
    {
        var syntaxTriviaList = SourceSyntax.GetTrailingTrivia();
        return syntaxTriviaList
            .FirstOrDefault(i => (SyntaxKind)i.RawKind is SyntaxKind.SingleLineCommentTrivia or SyntaxKind.MultiLineCommentTrivia );
    }

    public string GetterAccess => MethodModifiers(prop.GetMethod);
    public string SetterAccess => MethodModifiers(prop.SetMethod);
    
    private string MethodModifiers(IMethodSymbol? method) =>
        (method?.DeclaringSyntaxReferences[0].GetSyntax() as AccessorDeclarationSyntax)?.Modifiers.ToString() is {Length:>0} str ?
            str + " ":"";
}