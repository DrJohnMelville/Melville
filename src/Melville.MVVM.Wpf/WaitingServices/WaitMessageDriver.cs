using  System;
using System.Threading;
using Melville.Hacks;
using Melville.INPC;
using Melville.MVVM.WaitingServices;

namespace Melville.MVVM.Wpf.WaitingServices;

public partial class WaitMessageDriver :IWaitingService
{
  /// <summary>
  /// Message that updates infrequently on the wait screen.
  /// </summary>
  [AutoNotify] private string? waitMessage;
  /// <summary>
  /// An error message that shows in red at the bottom of the screen.
  /// Intended to be used after a failed operation.
  /// </summary>
  [AutoNotify] private string? errorMessage;
  /// <summary>
  /// A message that updates frequently on the progress screen showing the
  /// current task.
  /// </summary>
  [AutoNotify] private string? progressMessage;

  #region Progress

  /// <summary>
  /// Indicates that the wait screen should or should not be displayed.
  /// </summary>
  [AutoNotify]public bool ShowProgress => Total > double.MinValue;
  /// <summary>
  /// The maximum value the progress bar is progressing toward.
  /// </summary>
  [AutoNotify] private double total = double.MinValue;
   
  /// <summary>
  /// The ammount of progress made toward the goal of Total.
  /// </summary>
  [AutoNotify]private double progress;

  public void MakeProgress(string? item = null)
  {
    Progress++;
    ProgressMessage = item ?? progressMessage;
  }

  /// <summary>
  /// If a CancellationTokenSource is provided, the the UI will show a cancel button.
  /// </summary>
  [AutoNotify]public bool ShowCancelButton => CancellationTokenSource != null;
  [AutoNotify]public CancellationToken CancellationToken => CancellationTokenSource?.Token ?? CancellationToken.None;
  /// <summary>
  /// A cancellation token source that will enable the user to cancel the operation.
  /// </summary>
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