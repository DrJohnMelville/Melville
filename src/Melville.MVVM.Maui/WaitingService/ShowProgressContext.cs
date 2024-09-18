using System.Net.Mail;
using System;
using System.Numerics;
using System.Threading;
using System.Windows.Input;
using Melville.Hacks;
using Melville.INPC;
using Melville.MVVM.WaitingServices;
using Microsoft.Maui.Controls;

namespace Melville.MVVM.Maui.WaitingService;

public partial class ShowProgressContext(INavigation navigation) :IShowProgressContext
{
  /// <summary>
  /// A message that updates frequently on the progress screen showing the
  /// current task.
  /// </summary>
  [AutoNotify] private string? waitMessage;

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

  [AutoNotify] public double ScaledProgress => Total == 0.0?0: Progress / Total;

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

  public ICommand CancelCommand => new Command(_ => CancellationTokenSource?.Cancel());

    #endregion

    public void Dispose() => navigation.PopModalAsync();
}