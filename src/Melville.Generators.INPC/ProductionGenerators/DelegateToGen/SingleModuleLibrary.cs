using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class SingleModuleLibrary : IDocumentationLibrary
{
    private static XElement Empty = new XElement("xml");
    private readonly string path;
    private XElement? xml;

    public SingleModuleLibrary(string path)
    {
        this.path = path.Substring(0,path.Length-3)+"xml";
    }

    public string LookupDocumentationFor(ISymbol symbol)
    {
        var name = symbol.GetDocumentationCommentId()??"xml";
        return Xml()
            .Descendants("member")
            .Where(i => name.Equals(i.Attribute("name")?.Value, StringComparison.Ordinal))
            .Select(ReadOuterXml)
            .DefaultIfEmpty("")
            .First();
    }

    private string ReadOuterXml(XElement arg)
    {
        using var reader = arg.CreateReader();
        reader.MoveToContent();
        return reader.ReadOuterXml();
    }

    public XElement Xml()
    {
        lock (path)
        {
            return xml ??= LoadDoc();
        }
    }

    private XElement LoadDoc()
    {
        try
        {
            if (!File.Exists(path)) return Empty;
            using var stream = File.Open(path, FileMode.Open);
            return XElement.Load(stream);
        }
        catch (Exception)
        {
            return Empty;
        }
    }
}