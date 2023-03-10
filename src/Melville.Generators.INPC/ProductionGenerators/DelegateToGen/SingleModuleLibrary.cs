using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

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
        if (File.Exists(ret)) return ret;
        ret = refReplacer.Replace(ret, "$1");
        if (File.Exists(ret)) return ret;
        return null;
    }

    private static Regex refReplacer = new Regex("""
        [\\/]        # forward or backwad slash
        ref
        ([\\/])       #forward or backward slash + capture it for use in replacement
        (?!.*[\\/]) # Lookahead to ensure no subsequent forward or backward slashes.
        """, RegexOptions.IgnorePatternWhitespace);

    private IDocumentationLibrary LoadFromPathThatExists(string filePath)
    {
       using var stream = File.Open(filePath, FileMode.Open);
        return ParseXmlDocumentation(XmlReader.Create(stream));
    }

    private IDocumentationLibrary ParseXmlDocumentation(XmlReader reader)
    {
        if (!reader.Read()) return AlwaysUseSymbolMethod.Instance;
        var ret = new Dictionary<string, string>();
        while (true)
        {
            if (reader.NodeType == XmlNodeType.Element && 
                reader.Name.Equals("member", StringComparison.Ordinal) &&
                reader.GetAttribute("name") is { Length: > 0 } name)
            {
                ret.Add(name, reader.ReadOuterXml());
            }
            else
            {
                if (!reader.Read()) return new DictionaryLibrary(ret);
            }
        }
    }

    public IDocumentationLibrary OptimizedLibrary() => members ?? this;
}