using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

public class FieldSourceSymbol(
    IFieldSymbol field, GeneratorAttributeSyntaxContext context): IPropertySourceSymbol
{
    public string FieldName => field.Name;
    public string PropertyName { get; } = field.Name.ComputePropertyName();
    public ITypeSymbol Type => field.Type;
    public ISymbol DocumentationSource => field;

    public SyntaxNode SourceSyntax { get; } = context.TargetNode;

    public string PropertyAccessLevel => 
        new PropertyParametersParser(context.Attributes).Parse();

    public void TryWriteFieldDeclaration(CodeWriter target)
    {
        // do nothing -- the field declaration already exists.
    }

    public string GetterAccess => "";
    public string SetterAccess => "";
}