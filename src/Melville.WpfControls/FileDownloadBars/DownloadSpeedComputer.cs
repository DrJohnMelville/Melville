using System;
using Melville.INPC;
using Melville.SystemInterface.Time;

namespace Melville.WpfControls.FileDownloadBars;

public partial class DownloadSpeedComputer
{
    [FromConstructor] private IWallClock wallClock;
    private DateTime lastTime;
    private long priorBytes;

    public string ReportInterval(long totalBytes)
    {
        var ret = ShowSpeed(totalBytes - priorBytes, wallClock.CurrentLocalTime() - lastTime);
        lastTime = wallClock.CurrentLocalTime();
        priorBytes = totalBytes;
        return ret;
    }

    private static string[] unitNames = { "B/sec", "Kb/sec", "Mb/Sec", "Gb/sec" };
    private string ShowSpeed(long totalbytestransferred, TimeSpan timeElapsed) => 
        ShowSpeed(totalbytestransferred / timeElapsed.TotalSeconds);

    private string ShowSpeed(double rate, int units = 0) =>
        rate > 1024 && (units + 1) < unitNames.Length
            ? ShowSpeed(rate / 1024, units + 1)
            : $"{rate:###0.00} {unitNames[units]}";
}