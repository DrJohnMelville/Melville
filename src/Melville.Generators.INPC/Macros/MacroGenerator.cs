using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.Macros
{
    public class MacroGeneratorOld: ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
        }

        private void Generate(List<MacroRequest> msrRequests, GeneratorExecutionContext context)
        {
            var namer = new GeneratedFileUniqueNamer("MacroGen");
            foreach (var group in msrRequests.GroupBy(i=>i.NodeToEnclose))
            {
                context.RenderInType(namer, group.Key, group.Select(i => i.GeneratedCode));
            }
        }

        private static void AddAttributes(GeneratorExecutionContext context)
        {
        }
    }
}