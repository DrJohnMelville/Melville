using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.GenerationTools.DocumentationCopiers;

public class XmlFileDocumentationLibrary : IDocumentationLibrary
{
    private readonly string dllPath;
    private IDocumentationLibrary? members;

    public XmlFileDocumentationLibrary(string dllPath)
    {
        this.dllPath = dllPath;
    }

    public string LookupDocumentationFor(ISymbol symbol) => Members().LookupDocumentationFor(symbol);

    public IDocumentationLibrary Members()
    {
        lock (dllPath)
        {
            return members ??= ParseXmlDocumentation();
        }
    }

    private IDocumentationLibrary ParseXmlDocumentation()
    {
        if (dllPath.Length < 5) return AlwaysUseSymbolMethod.Instance;
        
        if (PathToDocumentationFile() is { } filePath)
        {
            try
            {
                return LoadFromPathThatExists(filePath);
            }
            catch (Exception)
            {
            }
        }

        return AlwaysUseSymbolMethod.Instance;
    }

    private string? PathToDocumentationFile()
    {
        var ret = dllPath.Substring(0, dllPath.Length - 3) + "xml";
        if (Exists(ret)) return ret;
        ret = TryRemoveTerminalRefFolderFromPath(ret);
        if (Exists(ret)) return ret;
        return null;
    }

    private bool Exists(string path) => new FileInfo(path).Exists;

    private static string TryRemoveTerminalRefFolderFromPath(string ret) => 
        refReplacer.Replace(ret, "$1");

    private static Regex refReplacer = new("""
        [\\/]        # forward or backwad slash
        ref
        ([\\/])       #forward or backward slash + capture it for use in replacement
        (?!.*[\\/]) # Lookahead to ensure no subsequent forward or backward slashes.
        """, RegexOptions.IgnorePatternWhitespace);

    private IDocumentationLibrary LoadFromPathThatExists(string filePath)
    {
       using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return new XmlDocumentationParser(XmlReader.Create(stream)).ParseXmlDocumentation();
    }

    public IDocumentationLibrary OptimizedLibrary() => members ?? this;
}