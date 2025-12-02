using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

public class FieldSourceSymbol(
    IFieldSymbol fieldSymbol, GeneratorAttributeSyntaxContext context): IPropertySourceSymbol
{
    public string FieldName => fieldSymbol.Name;
    public string PropertyName { get; } = fieldSymbol.Name.ComputePropertyName();
    public ITypeSymbol Type => fieldSymbol.Type;
    public ISymbol DocumentationSource => fieldSymbol;

    public SyntaxNode SourceSyntax { get; } = context.TargetNode;

    public string PropertyAccessLevel => 
        new PropertyParametersParser(context.Attributes).Parse();

    public void TryWriteFieldDeclaration(CodeWriter target)
    {
        // do nothing -- the fieldSymbol declaration already exists.
    }

    public string GetterAccess => "";
    public string SetterAccess => "";
}