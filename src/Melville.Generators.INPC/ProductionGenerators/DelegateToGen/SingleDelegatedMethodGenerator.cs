using System.Diagnostics.SymbolStore;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MemberGenerators;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;



public readonly struct SingleDelegatedMethodGenerator
{
    private readonly CodeWriter cw;
    private readonly ClassGenerator parent;

    public SingleDelegatedMethodGenerator(CodeWriter cw, ClassGenerator parent)
    {
        this.cw = cw;
        this.parent = parent;
    }

    public void GenerateForwardingMember(ISymbol member) => CreateMemberGenerator(member).WriteSymbol(cw);

    private IMemberGenerator CreateMemberGenerator(ISymbol member) => member switch
    {
        IPropertySymbol { IsIndexer: true } ps => new IndexerGenerator(ps, parent),
        IPropertySymbol ps => new PropertyGenerator(ps, parent, ps.Name),
        IEventSymbol es => new EventGenerator(es, parent, es.Name),
        IMethodSymbol ms => new MethodGenerator(ms, parent, ms.Name)
    };

}