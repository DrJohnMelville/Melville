using  System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Melville.MVVM.WaitingServices
{
  public interface IReportProgress
  {
    double Total { get; set; }
    double Progress { get; set; }
    void MakeProgress();
    CancellationToken CancellationToken { get; }
  }

  public interface IWaitingService : IReportProgress
  {
    IDisposable WaitBlock(string message, double maximum = double.MinValue, bool showCancelButton = false);
    string? WaitMessage { get; set; }
    string? ErrorMessage { get; set; }
  }
}
