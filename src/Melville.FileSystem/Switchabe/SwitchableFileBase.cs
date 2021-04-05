using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Melville.FileSystem.Switchabe
{
  public abstract class SwitchableFileBase: IFile, IHasLocalPath, INotifyPropertyChanged
  {
    protected IFile currentTarget;
    protected readonly IFile destination;

    protected SwitchableFileBase(IFile source, IFile destination)
    {
      currentTarget = source;
      this.destination = destination;
    }

    #region Delegate File Operations
    public IDirectory? Directory => currentTarget.Directory;
    public string Path => currentTarget.Path;
    public string Name => destination.Name;
    public string DestinationPath => currentTarget.Path;
    public long Size => currentTarget.Size;
    public DateTime LastAccess
    {
      get { return currentTarget.LastAccess; }
      set { currentTarget.LastAccess = value; }
    }
    public DateTime LastWrite => currentTarget.LastWrite;
    public DateTime Created => currentTarget.Created;
    public bool Exists() => currentTarget.Exists();
    public bool ValidFileSystemPath() => currentTarget.ValidFileSystemPath();
    public FileAttributes Attributes => currentTarget.Attributes;
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

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}