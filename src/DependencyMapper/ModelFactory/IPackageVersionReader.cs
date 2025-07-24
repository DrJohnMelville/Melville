using System.Xml.Linq;
using Melville.FileSystem;

namespace DependencyMapper.ModelFactory;

public interface IPackageVersionReader
{
    public string? VersionFor(XElement elt, string name);
}

public class AttributeVersionReader(string versionAttribute) : IPackageVersionReader
{
    /// <inheritdoc />
    public string? VersionFor(XElement elt, string name) => 
        elt.Attribute(versionAttribute)?.Value;
}
public class ChildVersionReader(string versionAttribute) : IPackageVersionReader
{
    /// <inheritdoc />
    public string? VersionFor(XElement elt, string name) => 
        elt.Element(versionAttribute)?.Value;
}

public class CompositeVersionReader(IPackageVersionReader[] strategies):IPackageVersionReader
{
    /// <inheritdoc />
    public string? VersionFor(XElement elt, string name)
    {
        foreach (var strategy in strategies)
        {
            if (strategy.VersionFor(elt, name) is { } version) return version;
        }
        return null;
    }
}

internal class GlobalVersionReaderFactory
{
    public static async Task<IEnumerable<IPackageVersionReader>> CreateFromFolder(
        IDirectory? projectFileDirectory)
    {
        var ret = new List<IPackageVersionReader>();
        for (var current = projectFileDirectory; current != null; current = current.Directory)
        {
            if (current.File("Directory.Packages.props") is { } packageFile &&
                packageFile.Exists())
            {
                var xml = (XElement)await packageFile.ReadAsXmlAsync();
                ret.Add(new GlobalVersionReader(ReadVersions(xml)));
            }
        }

        return ret;
    }

    private static IReadOnlyDictionary<string, string> ReadVersions(XElement xml)
    {
        return new Dictionary<string, string>(
            xml.Descendants("PackageVersion").
                Select(i=> new KeyValuePair<string, string>(
                    i.Attribute("Include")?.Value ?? string.Empty,
                    i.Attribute("Version")?.Value ?? string.Empty
                    ))
                .Where(i=>i is { Key: {Length:>0}, Value:{Length:>0}})
            );
    }
}

internal class GlobalVersionReader(IReadOnlyDictionary<string, string> versions) : IPackageVersionReader
{
    /// <inheritdoc />
    public string? VersionFor(XElement elt, string name) => 
        versions.GetValueOrDefault(name);
}