using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Melville.Generators.INPC.Test.UnitTests
{
    public class GeneratorTestBed
    {
        public GeneratorTestBed(ISourceGenerator sut, string code)
        {
            var sourceCompilation = CompileCode(code);
            var opt = new CSharpParseOptions(kind: SourceCodeKind.Regular, documentationMode: DocumentationMode.Parse);
            var driver = CSharpGeneratorDriver.Create(ImmutableArray.Create<ISourceGenerator>(sut), 
                ImmutableArray<AdditionalText>.Empty, opt);
            driver.RunGeneratorsAndUpdateCompilation(sourceCompilation, out compilation, out diagnostics);

        }

        private Compilation compilation;
        private ImmutableArray<Diagnostic> diagnostics;


        private static CSharpCompilation CompileCode(string code)
        {
            CSharpParseOptions parseOptions = new CSharpParseOptions()
                .WithKind(SourceCodeKind.Regular) // ...as representing a complete .cs file
                .WithLanguageVersion(LanguageVersion.Latest); // ...enabling the latest language features

            // Compile the C# code...
            CSharpCompilationOptions compileOptions =
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary) // ...to a dll
                    .WithOptimizationLevel(OptimizationLevel.Release) // ...in Release configuration
                    .WithAllowUnsafe(enabled: true); // ...enabling unsafe code

            // Invoke the compiler...
            CSharpCompilation compilation =
                CSharpCompilation.Create("TestInMemoryAssembly") // ..with some fake dll name
                    .WithOptions(compileOptions)
                    .AddReferences(
                        MetadataReference.CreateFromFile(typeof(object).Assembly
                            .Location)); // ...referencing the same mscorlib we're running on

            var tree = CSharpSyntaxTree.ParseText(code, parseOptions);
            compilation = compilation.AddSyntaxTrees(tree);
            // Parse and compile the C# code into a *.dll and *.xml file in-memory
            return compilation;
        }

        public void AssertNoDiagnostics()
        {
            Assert.Empty(diagnostics);
        }

        public void AssertDiagnosticCount(int expetedDiagnostics)
        {
            Assert.Equal(expetedDiagnostics, diagnostics.Length);
            
        }

        private SyntaxTree TreeFromName(string fileName) => 
            compilation.SyntaxTrees.First(i => i.FilePath.Contains(fileName));

        public void FileContains(string fileName, string fileContent)
        {
            Assert.Contains(fileContent, TreeFromName(fileName).ToString());
            
        }
        public void FileDoesNotContain(string fileName, string fileContent)
        {
            Assert.DoesNotContain(fileContent, TreeFromName(fileName).ToString());
            
        }

        public void FileEqual(string fileName, string fileContent)
        {
            Assert.Equal(fileContent, TreeFromName(fileName).ToString());
            
        }

        public void NoSuchFile(string fileName)
        {
            Assert.Empty(compilation.SyntaxTrees.Where(i=>i.FilePath.Contains(fileName)));
        }
    }
}