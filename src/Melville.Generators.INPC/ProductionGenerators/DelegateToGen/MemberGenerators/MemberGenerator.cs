using Microsoft.CodeAnalysis;
using System;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.ClassGenerators;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MemberGenerators
{
    public interface IMemberGenerator
    {
        bool IsSuppressedBy(ISymbol comparisonItem);
        void WriteSymbol(CodeWriter cw);
    }

    public abstract class MemberGenerator<T>: IMemberGenerator where T: ISymbol    {
        protected T SourceSymbol { get; }
        private readonly ClassGenerator host;
        private readonly string memberName;

        protected abstract ITypeSymbol ResultType { get; }
        protected abstract string CopiedAttributeName();
        protected virtual string EventElt()=>"";

        protected MemberGenerator(T sourceSymbol, ClassGenerator host, string memberName)
        {
            SourceSymbol = sourceSymbol;
            this.host = host;
            this.memberName = memberName;
        }

        public virtual bool IsSuppressedBy(ISymbol comparisonItem) => 
            comparisonItem.Name.Equals(memberName, StringComparison.Ordinal);

        public virtual void WriteSymbol(CodeWriter cw)
        {
            new DocumentationCopier(cw).Copy(SourceSymbol);
            CopySourceAttributes(cw);
            CopyApplicableHostAttributes(cw);
            host.RenderPrefix(cw, SourceSymbol.DeclaredAccessibility, EventElt(), ResultType, memberName);
        }

        private void CopySourceAttributes(CodeWriter cw) => 
            new AttributeCopier(cw, "").CopyAttributesFrom(SourceSymbol.DeclaringSyntaxReferences);

        private void CopyApplicableHostAttributes(CodeWriter cw) =>
            new AttributeCopier(cw, CopiedAttributeName()).CopyAttributesFrom(host.HostSymbol.DeclaringSyntaxReferences);

        protected void AppendHostSymbolAccess(CodeWriter cw) => cw.Append(host.MethodPrefix);
    }
}
