using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Melville.INPC;

namespace Melville.FileSystem.Switchabe;

public abstract partial class SwitchableFileBase: IFile, IHasLocalPath, INotifyPropertyChanged
{
  [DelegateTo] protected IFile currentTarget;
  protected readonly IFile destination;

  protected SwitchableFileBase(IFile source, IFile destination)
  {
    currentTarget = source;
    this.destination = destination;
  }

  #region Delegate File Operations
  public string Name => destination.Name;
  public LocalFileNamePackage? LocalPath() => (currentTarget as IHasLocalPath)?.LocalPath();
  public Task<Stream> CreateWrite(FileAttributes attributes = FileAttributes.Normal) => throw new NotSupportedException();
  public void Delete() => throw new NotSupportedException();
  #endregion

  public abstract Task<Stream> OpenRead();

  public bool IsSwitched => currentTarget == destination;
  protected readonly TaskCompletionSource<int> completionSource = new TaskCompletionSource<int>();
  public byte FinalProgress => IsSwitched? (byte) 255 : (byte) 0;
  public Task WaitForFinal => completionSource.Task;
  protected virtual Task CleanupAfterSwitch() => Task.CompletedTask;

  public abstract Task RelocateOverride();

  public async Task Relocate(Action<string> cancelAction)
  {
    try
    {
      if (IsSwitched) return;
      await RelocateOverride();
      SwitchFiles();
      await CleanupAfterSwitch();
    }
    catch (Exception e)
    {
      cancelAction("Transfer Failed: "+ e.Message);
    }
  }

  private void SwitchFiles()
  {
    if (IsSwitched) return;
    currentTarget = destination;
    completionSource.SetResult(1);
    OnPropertyChanged(nameof(FinalProgress));
  }

  public event PropertyChangedEventHandler? PropertyChanged;

  protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}