using  System;
using System.Threading;

namespace Melville.MVVM.WaitingServices;

public interface IReportProgress
{
  double Total { get; set; }
  double Progress { get; set; }
  string? ProgressMessage { get; set; }
  void MakeProgress(string? item = null);
  CancellationToken CancellationToken { get; }
}

public interface IWaitingService : IReportProgress
{
  IDisposable WaitBlock(string message, double maximum = double.MinValue, bool showCancelButton = false);
  string? WaitMessage { get; set; }
  string? ErrorMessage { get; set; }
}