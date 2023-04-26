using System.IO;
using System.Xml;
using Melville.Generators.INPC.GenerationTools.DocumentationCopiers;
using Microsoft.CodeAnalysis;
using Moq;
using Xunit;

namespace Melville.Generators.INPC.Test.DocumentationCopiers;

public class XmlDocumentationTest
{
    [Fact]
    public void RetrievesPropertyDocumentation()
    {
        var lib = ConstructLibrary("""
                <member name="T:Microsoft.Win32.SafeHandles.CriticalHandleMinusOneIsInvalid">
                  <summary>Provides a base class for Win32 critical handle implementations in which the value of -1 indicates an invalid handle.</summary>
                </member>
                <member name="M:Microsoft.Win32.SafeHandles.CriticalHandleMinusOneIsInvalid.#ctor">
                  <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.SafeHandles.CriticalHandleMinusOneIsInvalid" /> class.</summary>
                </member>            
            """);

        Assert.Equal("""
            <member name="T:Microsoft.Win32.SafeHandles.CriticalHandleMinusOneIsInvalid">
                  <summary>Provides a base class for Win32 critical handle implementations in which the value of -1 indicates an invalid handle.</summary>
                </member>
            """, lib.LookupDocumentationFor(SymbolNamed("T:Microsoft.Win32.SafeHandles.CriticalHandleMinusOneIsInvalid")).Replace("\n","\r\n"));
    }

    private ISymbol SymbolNamed(string name)
    {
        var ret = new Mock<IMethodSymbol>();
        ret.Setup(i => i.GetDocumentationCommentId()).Returns(name);
        return ret.Object;
    }

    private IDocumentationLibrary ConstructLibrary(string s)
    {
        var reader = XmlReader.Create(new StringReader($"""
            <?xml version="1.0" encoding="utf-8"?>
            <doc>
              <assembly>
                <name>System.Runtime</name>
              </assembly>
              <members>
                {s}
              </members>
            </doc>
            """));
        return new XmlDocumentationParser(reader).ParseXmlDocumentation();
    }
}