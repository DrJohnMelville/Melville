using  System;
using System.Threading;
using Melville.Hacks;
using Melville.INPC;
using Melville.MVVM.WaitingServices;

namespace Melville.MVVM.Wpf.WaitingServices;

public partial class WaitMessageDriver :IWaitingService
{
  [AutoNotify] private string? waitMessage;
  [AutoNotify] private string? errorMessage;
  [AutoNotify] private string? progressMessage;
  #region Progress

  [AutoNotify]public bool ShowProgress => Total > double.MinValue;
  [AutoNotify] private double total = double.MinValue;
   
  [AutoNotify]private double progress;

  public void MakeProgress(string? item = null)
  {
    Progress++;
    ProgressMessage = item ?? progressMessage;
  }

  [AutoNotify]public bool ShowCancelButton => CancellationTokenSource != null;
  [AutoNotify]public CancellationToken CancellationToken => CancellationTokenSource?.Token ?? CancellationToken.None;
  [AutoNotify]private CancellationTokenSource? cancellationTokenSource;
  public void CancelWaitOperation() => CancellationTokenSource?.Cancel();

  #endregion

  public IDisposable WaitBlock(string message, double maximum = Double.MinValue, bool showCancelButton = false)
  {
    WaitMessage = message;
    ErrorMessage = null;
    Progress = 0.0;
    Total = maximum;
    if (showCancelButton)
    {
      CancellationTokenSource = new CancellationTokenSource();
    }

    return new ActionOnDispose(CancelWaitBlock);
  }

  private void CancelWaitBlock()
  {
    WaitMessage = null;
    Total = double.MinValue;
    CancellationTokenSource = null;
  }
}