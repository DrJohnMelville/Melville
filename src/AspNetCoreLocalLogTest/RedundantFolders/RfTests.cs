using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.Log.Viewer.NugetMonitor;
using Melville.Log.Viewer.RedundantFolders;
using Melville.Mvvm.TestHelpers.MockFiles;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AspNetCoreLocalLogTest.RedundantFolders;

public class RfTests
{
    private Mock<ILogConsole> log = new();

    [Fact]
    public void FindNuPkg()
    {
        var folder = new MockDirectoryTreeBuilder("C:/release")
            .Folder("net10.0")
            .File("a.b.c.nupkg", "aa`")
            .File("a.b.c.nup1kg", "aa`")
            .Object;

        new RedundantFolderScanner(folder, log.Object, LogOnlyStrategy.Instance).Scan();

        log.Verify(i => i.WriteToLog("Found C:/release\\a.b.c.nupkg", LogLevel.Information));
        log.VerifyNoOtherCalls();

    }

    [Fact]
    public void RecurseForFolder()
    {
        var folder = new MockDirectoryTreeBuilder("C:/B")
            .Folder("A", s=>s.Folder("Debug", i=>i.Folder("net9.0")
            .Folder("net10.0")))
            .Object;

        new RedundantFolderScanner(folder, log.Object, LogOnlyStrategy.Instance).Scan();

        log.Verify(i => i.WriteToLog("Found C:/B\\A\\Debug\\net9.0", LogLevel.Information));
        log.VerifyNoOtherCalls();
    }
    [Fact]
    public void SingleFolder()
    {
        var folder = new MockDirectoryTreeBuilder("C:/Debug")
            .Folder("net9.0")
            .Folder("net10.0")
            .Object;

        new RedundantFolderScanner(folder, log.Object, LogOnlyStrategy.Instance).Scan();

        log.Verify(i => i.WriteToLog("Found C:/Debug\\net9.0", LogLevel.Information));
        log.VerifyNoOtherCalls();
    }
    [Fact]
    public void WrongFolderName()
    {
        var folder = new MockDirectoryTreeBuilder("C:/Debug2")
            .Folder("net9.0")
            .Folder("net10.0")
            .Object;

        new RedundantFolderScanner(folder, log.Object, LogOnlyStrategy.Instance).Scan();

        log.VerifyNoOtherCalls();
    }
}
