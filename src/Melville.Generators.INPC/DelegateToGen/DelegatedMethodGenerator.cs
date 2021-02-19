using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.DelegateToGen
{
    public interface IDelegatedMethodGenerator
    {
        string? InheritFrom();
        void GenerateForwardingMethods(ITypeSymbol parentClass, CodeWriter cw);
    }
    
    public abstract class DelegatedMethodGenerator : IDelegatedMethodGenerator
    {
        protected readonly ITypeSymbol TargetType;
        protected readonly string MethodPrefix;

        public static IDelegatedMethodGenerator Create(
            ITypeSymbol targetType, string methodPrefix, SyntaxNode location) =>
            targetType.TypeKind switch
            {
                TypeKind.Interface => new InterfaceMethodGenerator(targetType, methodPrefix),
                _ => new InvalidParentMethodGenerator(targetType, location)
            };

        protected DelegatedMethodGenerator(ITypeSymbol targetType, string methodPrefix)
        {
            this.TargetType = targetType;
            this.MethodPrefix = methodPrefix;
        }

        public abstract string InheritFrom();

        public abstract void GenerateForwardingMethods(ITypeSymbol parentClass, CodeWriter cw);
    }
}