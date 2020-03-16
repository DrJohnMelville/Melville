using  System;
using System.Threading;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.CSharpHacks;
using Melville.MVVM.WaitingServices;

namespace Melville.MVVM.Wpf.WaitingServices
{
  public class WaitMessageDriver : NotifyBase, IWaitingService
  {
    private string? waitMessage;
    public string? WaitMessage
    {
      get { return waitMessage; }
      set { AssignAndNotify(ref waitMessage, value); }
    }

    private string? errorMessage;
    public string? ErrorMessage
    {
      get { return errorMessage; }
      set { AssignAndNotify(ref errorMessage, value); }
    }

    #region Progress

    public bool ShowProgress => Total > double.MinValue;
    private double total = double.MinValue;
    public double Total
    {
      get { return total; }
      set { AssignAndNotify(ref total, value, nameof(Total), nameof(ShowProgress)); }
    }
    private double progress;
    public double Progress
    {
      get { return progress; }
      set { AssignAndNotify(ref progress, value, nameof(Progress)); }
    }

    public void MakeProgress()
    {
      Progress++;
    }

    public bool ShowCancelButton => CancellationTokenSource != null;
    public CancellationToken CancellationToken => CancellationTokenSource?.Token ?? CancellationToken.None;
    private CancellationTokenSource? cancellationTokenSource;
    public CancellationTokenSource? CancellationTokenSource
    {
      get => cancellationTokenSource;
      set => AssignAndNotify(ref cancellationTokenSource, value, nameof(CancellationTokenSource),
        nameof(ShowCancelButton),
        nameof(CancellationToken));
    }
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
}