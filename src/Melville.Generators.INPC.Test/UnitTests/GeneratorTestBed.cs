using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.ProductionGenerators.INPC;
using Melville.INPC;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Melville.Generators.INPC.Test.UnitTests;

public class GeneratorTestBed
{
    public GeneratorTestBed(ISourceGenerator sut, string code, params Type[] typesToInclude) : this(
        CSharpGeneratorDriver.Create(ImmutableArray.Create(sut), ImmutableArray<AdditionalText>.Empty,
            CSharpParseOptions), code, typesToInclude)
    {
    }

    public GeneratorTestBed(IIncrementalGenerator sut, string code, params Type[] typesToInclude) : this(
        CSharpGeneratorDriver.Create(sut), code, typesToInclude)
    {
    }
    public GeneratorTestBed(CSharpGeneratorDriver driver, string code, params Type[] typesToInclude)
    {
        var sourceCompilation = CompileCode(code, typesToInclude);
        driver.RunGeneratorsAndUpdateCompilation(sourceCompilation, out  compilation, out diagnostics);
    }

    private readonly Compilation compilation;
    private ImmutableArray<Diagnostic> diagnostics;
    private static readonly CSharpParseOptions CSharpParseOptions = new CSharpParseOptions(kind: SourceCodeKind.Regular, documentationMode: DocumentationMode.Parse);


    private static CSharpCompilation CompileCode(string code, Type[] typesToInclude)
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
                .AddReferences(SystemReferences())
                .AddReferences(AllReferences(typesToInclude));

        var tree = CSharpSyntaxTree.ParseText(code, parseOptions);
        compilation = compilation.AddSyntaxTrees(tree);
        // Parse and compile the C# code into a *.dll and *.xml file in-memory
        return compilation;
    }

    // allows loading the attributes from another assembly.  See
    // https://stackoverflow.com/questions/23907305/roslyn-has-no-reference-to-system-runtime/72618941#72618941
    // https://stackoverflow.com/questions/68231332/in-memory-csharpcompilation-cannot-resolve-attributes
    private static IEnumerable<MetadataReference> SystemReferences()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(i => i is { IsDynamic: false, Location: { Length: > 0 } })
            .Select(i => MetadataReference.CreateFromFile(i.Location));
    }

    private static IEnumerable<PortableExecutableReference> AllReferences(Type[] typesToInclude) => 
        typesToInclude
            .Select(i => i.Assembly.Location)
            .Distinct()
            .Select(i=>MetadataReference.CreateFromFile(i));

    public void AssertNoDiagnostics() => Assert.Empty(diagnostics);

    public void AssertDiagnosticCount(int expetedDiagnostics) => 
        Assert.Equal(expetedDiagnostics, diagnostics.Length);

    public FileAsserts FromName(string fileName) => 
        new(compilation.SyntaxTrees.First(i => i.FilePath.Contains(fileName)));
    public FileAsserts LastFile() => new(compilation.SyntaxTrees.Last());

    public void NoSuchFile(string fileName)
    {
        Assert.Empty(compilation.SyntaxTrees.Where(i=>i.FilePath.Contains(fileName)));
    }
}