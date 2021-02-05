using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.INPC
{
    public class InpcClassGeneratorFactory
    {
        private readonly INamedTypeSymbol stringSymbol;

        private readonly ISet<INamedTypeSymbol> allTypes = new HashSet<INamedTypeSymbol>(
            SymbolEqualityComparer.Default);

        public InpcClassGeneratorFactory(IEnumerable<ClassToImplement> allClasses,
            INamedTypeSymbol stringSymbol)
        {
            this.stringSymbol = stringSymbol;
            CatalogAllClassesToBeEnhanced(allClasses);
        }

        private void CatalogAllClassesToBeEnhanced(IEnumerable<ClassToImplement> allClasses)
        {
            foreach (var implClass in allClasses)
            {
                allTypes.Add(implClass.TypeInfo);
            }
        }

        public InpcClassGenerator CreateGenerator(ClassToImplement target, GeneratorExecutionContext context) => 
            new(target, StrategyForClass(target.TypeInfo), context);

        private INotifyImplementationStategy StrategyForClass(INamedTypeSymbol target)
        {
            if (HasOnPropertyChangedMethod(target)) return new HasMethodStrategy();
            if (CompatibleExplicitInterfaceDeclaration(target.BaseType) is {} baseName)
                return new UseInterfaceStrategy(baseName.FullyQualifiedName());
            return DeclareOrUseNotificationInterface(target);
        }

        private INotifyImplementationStategy DeclareOrUseNotificationInterface(INamedTypeSymbol target)
        {
            var generatedRoot = MostGeneralAncestorThatWillBeGenerated(target) ?? target;
            var intName = ExplicitlyDeclaredInterfaceName(generatedRoot)??
                          "Melville.INPC.IExternalNotifyPropertyChanged";
            return NeedToImplementINPC(target, generatedRoot, intName)
                ? new DeclareInterfaceStrategy(intName)
                : new UseInterfaceStrategy(intName);
        }

        private string? ExplicitlyDeclaredInterfaceName(INamedTypeSymbol generatedRoot) => 
            CompatibleExplicitInterfaceDeclaration(generatedRoot)?.FullyQualifiedName();

        private bool NeedToImplementINPC(INamedTypeSymbol target, INamedTypeSymbol generatedRoot, 
            string intName) =>
            SymbolEqualityComparer.Default.Equals(generatedRoot, target)
            && MemberIsMissing(generatedRoot, intName + ".OnPropertyChanged");

        private bool MemberIsMissing(INamedTypeSymbol generatedRoot, string methodName) => 
            !generatedRoot
                .GetMembers(methodName)
                .OfType<IMethodSymbol>()
                .Any(i => HasSingleStringParameter(i.Parameters));

        private INamedTypeSymbol? MostGeneralAncestorThatWillBeGenerated(INamedTypeSymbol? child)
        {
            if (child == null) return null;
            return MostGeneralAncestorThatWillBeGenerated(child.BaseType) ??
                   (allTypes.Contains(child) ? child : null);
        }

        private INamedTypeSymbol? CompatibleExplicitInterfaceDeclaration(INamedTypeSymbol? target) => 
            target?.Interfaces
                .Where(HasOnPropertyChangedMethod)
                .FirstOrDefault();

        private bool HasOnPropertyChangedMethod(INamedTypeSymbol? target)
        {
            if (target == null) return false;
            return target.GetMembers().OfType<IMethodSymbol>().Any(IsOnPropertyChangedMethod) ||
                   HasOnPropertyChangedMethod(target.BaseType);
        }

        private bool IsOnPropertyChangedMethod(IMethodSymbol i) =>
            i.Name.Equals("OnPropertyChanged", StringComparison.Ordinal) &&
            HasSingleStringParameter(i.Parameters);

        private bool HasSingleStringParameter(ImmutableArray<IParameterSymbol> parameters) =>
            parameters.Length == 1 && IsStringParameter(parameters[0]);

        private bool IsStringParameter(IParameterSymbol parameter) =>
            SymbolEqualityComparer.Default.Equals(parameter.Type, stringSymbol);
    }
}