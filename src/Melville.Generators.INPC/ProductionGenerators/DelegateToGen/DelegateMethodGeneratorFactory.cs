using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public static class DelegateMethodGeneratorFactory
{
    public static IDelegatedMethodGenerator Create(
        ITypeSymbol targetType, string methodPrefix, ITypeSymbol parent, bool explicitImplementation) =>
        (targetType.TypeKind, explicitImplementation) switch
        {
            (TypeKind.Interface, true) =>
                new ExplicitMethodGenerator(targetType, methodPrefix,
                    targetType.FullyQualifiedName() + ".", parent),
            (TypeKind.Interface, _) => new InterfaceMethodGenerator(targetType, methodPrefix, parent),
            (TypeKind.Class, _) => new BaseClassMethodGenerator(targetType, methodPrefix, parent),
            _ => new InvalidParentMethodGenerator(targetType,
                parent.DeclaringSyntaxReferences.First().GetSyntax())
        };
}