using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

public static class NotifyImplementationStrategyFactory
{
    public static INotifyImplementationStategy StrategyForClass(ITypeSymbol target)
    {
        if (HasOnPropertyChangedMethod(target)) return new HasMethodStrategy();
        if (CompatibleExplicitInterfaceDeclaration(target.BaseType) is {} baseName)
            return new UseInterfaceStrategy(baseName.FullyQualifiedName());
        return DeclareOrUseNotificationInterface(target);
    }

    private static INotifyImplementationStategy DeclareOrUseNotificationInterface(ITypeSymbol target)
    {
        var generatedRoot = MostGeneralAncestorThatWillBeGenerated(target) ?? target;
        var intName = ExplicitlyDeclaredInterfaceName(generatedRoot)??
                      "Melville.INPC.IExternalNotifyPropertyChanged";
        return NeedToImplementINPC(target, generatedRoot, intName)
            ? new DeclareInterfaceStrategy(intName)
            : new UseInterfaceStrategy(intName);
    }

    private static string? ExplicitlyDeclaredInterfaceName(ITypeSymbol generatedRoot) => 
        CompatibleExplicitInterfaceDeclaration(generatedRoot)?.FullyQualifiedName();

    private static bool NeedToImplementINPC(ITypeSymbol target, ITypeSymbol generatedRoot, 
        string intName) =>
        SymbolEqualityComparer.Default.Equals(generatedRoot, target)
        && MemberIsMissing(generatedRoot, intName + ".OnPropertyChanged");

    private static bool MemberIsMissing(ITypeSymbol generatedRoot, string methodName) =>
        !generatedRoot.HasMethod(null,methodName, typeof(string));
       
    private static ITypeSymbol? MostGeneralAncestorThatWillBeGenerated(ITypeSymbol? child)
    {
        if (child == null) return null;
        return MostGeneralAncestorThatWillBeGenerated(child.BaseType) ??
               (RequiresInpcGeneration(child) ? child : null);
    }

    private static bool RequiresInpcGeneration(ITypeSymbol child) =>
        TypeAndAllItsMembers(child).Any(NeedsAutoINPCImplementation);

    private static IEnumerable<SyntaxReference> TypeAndAllItsMembers(ITypeSymbol child) =>
        child.GetMembers()
            .SelectMany(i => i.DeclaringSyntaxReferences)
            .Concat(child.DeclaringSyntaxReferences);

    private static bool NeedsAutoINPCImplementation(SyntaxReference i) => 
        GetDeclarationSyntax(i.GetSyntax()) is { } mds && attrFinder.HasAttribute(mds);

    private static MemberDeclarationSyntax? GetDeclarationSyntax(SyntaxNode node) =>
        node.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().FirstOrDefault();

    private static readonly SearchForAttribute attrFinder = new("Melville.INPC.AutoNotifyAttribute");

    private static ITypeSymbol? CompatibleExplicitInterfaceDeclaration(ITypeSymbol? target) => 
        target?.Interfaces
            .Where(HasOnPropertyChangedMethod)
            .FirstOrDefault();

    private static bool HasOnPropertyChangedMethod(ITypeSymbol target) => 
        target.HasMethod(null, "OnPropertyChanged", typeof(string));
}