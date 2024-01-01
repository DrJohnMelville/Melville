using System.Runtime.InteropServices;

namespace Melville.Log.Viewer.NugetMonitor.HardDelete;

/// <summary>
/// Uniquely identifies a process by its PID and the time the process began.
/// An array of RM_UNIQUE_PROCESS structures can be passed to the RmRegisterResources function. 
/// </summary>
[StructLayout(LayoutKind.Sequential)]
// ReSharper disable once InconsistentNaming
public struct RM_UNIQUE_PROCESS
{
    public int dwProcessId;
    public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
}