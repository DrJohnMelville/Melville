using System;
using System.IO;
using Melville.FileSystem;
using System.Diagnostics;
using System.Linq;


namespace Melville.Log.Viewer.NugetMonitor.HardDelete;

public static class HardFileDelete
{
    public static void HardDelete(this IFile file)
    {
        for (int i = 0; i < 5; i++)
        {
            try
            {
                file.Delete();
                return;
            }
            catch (UnauthorizedAccessException)
            {
                TerminateProcessFor(file.Path);
            }
        }
    }

    private static void TerminateProcessFor(string filePath)
    {
        foreach (var lockingProcessId in FindLockerProcesses(filePath)
                     .Select(i => i.Process.dwProcessId)
                     .Distinct())
        {
            Process.GetProcessById(lockingProcessId).Kill();
        }
    }

    public static RM_PROCESS_INFO[] FindLockerProcesses(string path)
    {
        if (NativeMethods.RmStartSession(out var handle, 0, strSessionKey: Guid.NewGuid().ToString()) !=
            RmResult.ERROR_SUCCESS)
            throw new Exception("Could not begin session. Unable to determine file lockers.");

        try
        {
            string[] resources = { path }; // Just checking on one resource.

            if (NativeMethods.RmRegisterResources(handle, (uint)resources.LongLength, resources, 0, null, 0, null) !=
                RmResult.ERROR_SUCCESS)
                throw new Exception("Could not register resource.");

            // The first try is done expecting at most ten processes to lock the file.
            uint arraySize = 10;
            RmResult result;
            do
            {
                var array = new RM_PROCESS_INFO[arraySize];
                uint arrayCount;
                result = NativeMethods.RmGetList(
                    handle, out arrayCount, ref arraySize, array, out _);
                if (result == RmResult.ERROR_SUCCESS)
                {
                    // Adjust the array length to fit the actual count.
                    Array.Resize(ref array, (int)arrayCount);
                    return array;
                }
                else if (result == RmResult.ERROR_MORE_DATA)
                {
                    // We need to initialize a bigger array. We only set the size, and do another iteration.
                    // (the out parameter arrayCount contains the expected value for the next try)
                    arraySize = arrayCount;
                }
                else
                {
                    throw new Exception("Could not list processes locking resource. Failed to get size of result.");
                }
            } while (result != RmResult.ERROR_SUCCESS);
        }
        finally
        {
            NativeMethods.RmEndSession(handle);
        }

        return [];
    }
}