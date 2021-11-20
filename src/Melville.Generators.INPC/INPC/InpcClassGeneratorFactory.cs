using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.INPC;

public static class InpcClassGeneratorFactory
{

    public static InpcClassGenerator CreateGenerator(ClassToImplement target, CodeWriter context) => 
        new(target, StrategyForClass(target.TypeInfo), context);

    private static INotifyImplementationStategy StrategyForClass(INamedTypeSymbol target)
    {
        if (HasOnPropertyChangedMethod(target)) return new HasMethodStrategy();
        if (CompatibleExplicitInterfaceDeclaration(target.BaseType) is {} baseName)
            return new UseInterfaceStrategy(baseName.FullyQualifiedName());
        return DeclareOrUseNotificationInterface(target);
    }

    private static INotifyImplementationStategy DeclareOrUseNotificationInterface(INamedTypeSymbol target)
    {
        var generatedRoot = MostGeneralAncestorThatWillBeGenerated(target) ?? target;
        var intName = ExplicitlyDeclaredInterfaceName(generatedRoot)??
                      "Melville.INPC.IExternalNotifyPropertyChanged";
        return NeedToImplementINPC(target, generatedRoot, intName)
            ? new DeclareInterfaceStrategy(intName)
            : new UseInterfaceStrategy(intName);
    }

    private static string? ExplicitlyDeclaredInterfaceName(INamedTypeSymbol generatedRoot) => 
        CompatibleExplicitInterfaceDeclaration(generatedRoot)?.FullyQualifiedName();

    private static bool NeedToImplementINPC(INamedTypeSymbol target, INamedTypeSymbol generatedRoot, 
        string intName) =>
        SymbolEqualityComparer.Default.Equals(generatedRoot, target)
        && MemberIsMissing(generatedRoot, intName + ".OnPropertyChanged");

    private static bool MemberIsMissing(INamedTypeSymbol generatedRoot, string methodName) =>
        !generatedRoot.HasMethod(null,methodName, typeof(string));
       
    private static INamedTypeSymbol? MostGeneralAncestorThatWillBeGenerated(INamedTypeSymbol? child)
    {
        if (child == null) return null;
        return MostGeneralAncestorThatWillBeGenerated(child.BaseType) ??
               (RequiresInpcGeneration(child) ? child : null);
    }

    private static bool RequiresInpcGeneration(INamedTypeSymbol child) =>
        TypeAndAllItsMembers(child).Any(NeedsAutoINPCImplementation);

    private static IEnumerable<SyntaxReference> TypeAndAllItsMembers(INamedTypeSymbol child) =>
        child.GetMembers()
            .SelectMany(i => i.DeclaringSyntaxReferences)
            .Concat(child.DeclaringSyntaxReferences);

    private static bool NeedsAutoINPCImplementation(SyntaxReference i) => 
        GetDeclarationSyntax(i.GetSyntax()) is { } mds && attrFinder.HasAttribute(mds);

    private static MemberDeclarationSyntax? GetDeclarationSyntax(SyntaxNode node) =>
        node.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().FirstOrDefault();

    private static readonly SearchForAttribute attrFinder = new("Melville.INPC.AutoNotifyAttribute");

    private static INamedTypeSymbol? CompatibleExplicitInterfaceDeclaration(INamedTypeSymbol? target) => 
        target?.Interfaces
            .Where(HasOnPropertyChangedMethod)
            .FirstOrDefault();

    private static bool HasOnPropertyChangedMethod(INamedTypeSymbol target) => 
        target.HasMethod(null, "OnPropertyChanged", typeof(string));
}