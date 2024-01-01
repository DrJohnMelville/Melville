using System;
using System.IO;
using Melville.FileSystem;

namespace Melville.Log.Viewer.NugetMonitor;

public class NugetServiceFolderMonitor
{
    private readonly FileSystemWatcher watcher;

    public NugetServiceFolderMonitor(IDirectory dir, ILogConsole console, Func<ILogConsole,NugetCacheEditor> editor)
    {
        console.WriteToLog($"Monitoring Nuget Folder: {dir.Path}");
        watcher = new FileSystemWatcher(dir.Path, "*.nupkg");
        AttachToWatcher(editor(console));
        console.WindowClosed += OnWindowClosed;
    }

    private void AttachToWatcher(NugetCacheEditor editor)
    {
        watcher.Deleted += editor.FileNotify;
        watcher.EnableRaisingEvents = true;
    }
    private void OnWindowClosed(object? sender, EventArgs e)
    {
        watcher.EnableRaisingEvents = false;
        watcher.Dispose();
    }
}