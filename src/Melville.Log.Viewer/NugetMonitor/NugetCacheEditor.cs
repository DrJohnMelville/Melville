using System;
using System.IO;
using Melville.FileSystem;
using Melville.Log.Viewer.NugetMonitor.HardDelete;
using Serilog.Events;

namespace Melville.Log.Viewer.NugetMonitor;


public sealed class NugetCacheEditor
{
    private readonly ILogConsole console;
    private readonly IDirectory cacheFolder;

    public NugetCacheEditor(ILogConsole console, Func<Environment.SpecialFolder, string, IDirectory> specialFolder)
    {
        this.console = console;
        cacheFolder = specialFolder(Environment.SpecialFolder.UserProfile, "\\.nuget\\packages");
        console.WriteToLog($"Nuget Package Cache at: {cacheFolder.Path}");
    }

    public void FileNotify(object sender, FileSystemEventArgs e)
    {
        if (e.Name is not null) UpdateLibrary(e.Name);
    }

    public void UpdateLibrary(string packageName)
    {
        console.WriteToLog($"Updating {packageName}");
        TryClearVersionFolder(packageName);
    }

    private void TryClearVersionFolder(string packageName)
    {
        foreach (var packageFolder in cacheFolder.AllSubDirectories())
        {
            if (!packageName.StartsWith(packageFolder.Name, StringComparison.OrdinalIgnoreCase)) 
                continue;
            foreach (var versionFolder in packageFolder.AllSubDirectories())
            {
                string finalName = $"{packageFolder.Name}.{versionFolder.Name}.nupkg";
                if (finalName.Equals(packageName, StringComparison.OrdinalIgnoreCase) &&
                    ExtraCheckToOnlyDeleteNugetFolders(packageName, versionFolder))
                {
                    RemoveFolder(packageFolder, versionFolder);
                    return;
                }
            }
        }
        console.WriteToLog($"Cound not find package {packageName} in cache.",
            LogEventLevel.Warning);
    }

    private static bool ExtraCheckToOnlyDeleteNugetFolders(
        string packageName, IDirectory versionFolder) => 
        versionFolder.File(packageName).Exists();

    private void RemoveFolder(IDirectory packageFolder, IDirectory versionFolder)
    {
        RecursiveDeleteFolder(versionFolder);
    }

    private void RecursiveDeleteFolder(IDirectory folder)
    {
        foreach (var subFolder in folder.AllSubDirectories())
        {
            RecursiveDeleteFolder(subFolder);
        }

        foreach (var file in folder.AllFiles())
        {
            file.HardDelete();
        }
        folder.Delete();
    }
}