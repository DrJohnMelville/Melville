using Melville.FileSystem;
using Melville.INPC;
using Melville.Log.Viewer.NugetMonitor;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Melville.Log.Viewer.RedundantFolders;

public interface IHandleRedundantFileObject
{
    void HandleItem(IFileSystemObject item, ILogConsole console);
}

[StaticSingleton]
public partial class LogOnlyStrategy : IHandleRedundantFileObject
{
    public void HandleItem(IFileSystemObject item, ILogConsole console)
    {
        console.WriteToLog($"Found {item.Path}", LogLevel.Information);
    }
}
[StaticSingleton]
public partial class LogAndDeleteStrategy : IHandleRedundantFileObject
{
    public void HandleItem(IFileSystemObject item, ILogConsole console)
    {
        console.WriteToLog($"Deleting {item.Path}", LogLevel.Information);
        DeleteRecursive(item);
    }

    private void DeleteRecursive(IFileSystemObject item)
    {
        if (item is IDirectory folder)
        {
            foreach (var dir in folder.AllSubDirectories()) DeleteRecursive(dir);
            foreach (var file in folder.AllFiles()) DeleteRecursive(file);
        }
        item.Delete();
    }
}

public partial class RedundantFolderScanner(IDirectory root, ILogConsole console, IHandleRedundantFileObject strategy)
{
    public void Scan()
    {
        foreach (var dir in RecursiveSearch(root))
        {
            strategy.HandleItem(dir, console);
        }
    }

    public IEnumerable<IFileSystemObject> RecursiveSearch(IDirectory root)
    {
        var priors = Priors(root);
        if (priors is not null)
        {
            return priors.Concat(NugetPackages(root));
        }
        else
        {
            return root.
                AllSubDirectories()
                .SelectMany(RecursiveSearch);
        }

    }

    private IEnumerable<IFileSystemObject> NugetPackages(IDirectory root) =>
        root.AllFiles("*.nupkg");

    public IEnumerable<IFileSystemObject>? Priors(IDirectory root)
    {
        if (!ParentRecognizer().IsMatch(root.Name)) return null;
        var ret = Candidates(root).ToList();
        if (ret.Count == 0) return null;
        var max = ret.Max(i => i.Number);
        return ret
            .Where(i => i.Number < max)
            .Select(i => i.dir);
    }
    public IEnumerable<(IFileSystemObject dir, double Number)> Candidates(IDirectory root)
    {
        foreach (var sub in root.AllSubDirectories())
        {
            if (DirRecognizer().Match(sub.Name) is { Success: true} match &&
                double.TryParse(match.Groups[1].ValueSpan, out var number))
            {
                yield return (sub, number);
            }
        }
    } 

    [GeneratedRegex(@"^net(\d+\.\d)")]
    public static partial Regex DirRecognizer();

    [GeneratedRegex("^(?:[Dd]ebug|[Rr]elease)$")]
    public static partial Regex ParentRecognizer();
}
