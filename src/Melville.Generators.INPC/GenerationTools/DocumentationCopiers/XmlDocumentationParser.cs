using System;
using System.Collections.Generic;
using System.Xml;

namespace Melville.Generators.INPC.GenerationTools.DocumentationCopiers;

public readonly struct XmlDocumentationParser
{
    private readonly XmlReader reader;

    public XmlDocumentationParser(XmlReader reader)
    {
        this.reader = reader;
    }

    public IDocumentationLibrary ParseXmlDocumentation()
    {
        if (!reader.Read()) return AlwaysUseSymbolMethod.Instance;
        var ret = new Dictionary<string, string>();
        while (true)
        {
            if (IsMemberNode() &&
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

    private bool IsMemberNode()
    {
        return reader.NodeType == XmlNodeType.Element &&
               reader.Name.Equals("member", StringComparison.Ordinal);
    }
}