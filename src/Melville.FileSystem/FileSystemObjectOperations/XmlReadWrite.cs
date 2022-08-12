using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Melville.FileSystem;

public static partial class FileOperations
{
    public static async Task<XNode> ReadAsXmlAsync(this IFile file)
    {
        await using var stream = await file.OpenRead();
        return await ReadAsXmlAsync(stream);
    }

    public static async Task<XNode> ReadAsXmlAsync(this Stream stream)
    {
        var xmlReader = XmlReader.Create(stream, new XmlReaderSettings(){Async = true});
        await xmlReader.MoveToContentAsync();
        return await XNode.ReadFromAsync(xmlReader, CancellationToken.None);
    }

    public static async Task WriteXmlAsync(this IFile file, XElement element)
    {
        await using var output = await file.CreateWrite();
        await WriteXmlAsync(output, element);
    }

    public static async Task WriteXmlAsync(this Stream output, XElement element)
    {
        using var writer = CreateXmlWriter(output);
        await element.WriteToAsync(writer, CancellationToken.None);
    }

    public static XmlWriter CreateXmlWriter(Stream output) =>
        XmlWriter.Create(output,
            new XmlWriterSettings
            {
                CheckCharacters = true,
                CloseOutput = false,
                ConformanceLevel = ConformanceLevel.Document,
                Encoding = Encoding.UTF8,
                Indent = false,
                NewLineHandling = NewLineHandling.Replace,
                Async = true
            });
}