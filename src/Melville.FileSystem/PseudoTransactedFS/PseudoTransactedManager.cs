using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Melville.Linq;

namespace Melville.FileSystem.PseudoTransactedFS;

/// <summary>
/// Used to show download progress with downloading from WinId4Web
/// </summary>
public interface IDownloadProgressStore: INotifyPropertyChanged
{
  long BytesRead { get; }
  long BytesToRead { get; }
  long BytesWritten { get; }
  long BytesToWrite { get; }
  bool ShowUI { get; }
}


public interface ITransactableStore: IAsyncDisposable
{
  IDisposable? WriteToken();
  IDirectory UntransactedRoot { get; }
  ITransactedDirectory BeginTransaction();
  bool IsLocalStore { get; }
  IDownloadProgressStore? ProgressStore { get; }

  Task<bool> RenewLease();
  //The DisposeAsync method inherited from IAsycDisposable ia used to
  // surrender the lease in some of the web file store decendents.
}

public class TransactionStoreBase 
{
  protected readonly IDirectory innerFileSystem;
  public TransactionStoreBase(IDirectory innerFileSystem)
  {
    this.innerFileSystem = innerFileSystem;
  }

  public IDirectory UntransactedRoot => innerFileSystem;
  public IDisposable? WriteToken() => innerFileSystem.SubDirectory("Audio").WriteToken();
  public bool IsLocalStore => true;
  public IDownloadProgressStore? ProgressStore => null;
  public ValueTask DisposeAsync() => new ValueTask();
  public Task<bool> RenewLease() => Task.FromResult(true);
}

public sealed class PseudoTransactedStore : TransactionStoreBase, ITransactableStore
{

  private bool isRepaired; // = false

  public PseudoTransactedStore(IDirectory innerFileSystem): base(innerFileSystem)
  {
    if (innerFileSystem.IsVolitleDirectory())
    {
      throw new InvalidDataException("It makes no sense to enable transactions on a volitile store.");
    }
    isRepaired = !innerFileSystem.AllFiles("*.txn").Any();
  }
    
  public ITransactedDirectory BeginTransaction()
  {
    Debug.Assert(isRepaired);
    return new TransactedDirectory(innerFileSystem);
  }

  #region Repair any incomplete transaction state
  public Task RepairIncompleteTransactions()
  {
    if (!innerFileSystem.Exists())
    {
      isRepaired = true;
      return Task.FromResult(1);
    }
    var list = new Dictionary<int, RepairTransaction>();
    var dirs = new[] {innerFileSystem}.SelectRecursive(i => i.AllSubDirectories());
#warning  use a generated regex
        var unpackTxnFile = new Regex(@"^(.+)\\([^\\]+)\.(\d+)\.txn$");
    
    foreach (var directory in dirs)
    {
      var files = Enumerable.ToArray<IFile>(directory.AllFiles("*.txn"));
      foreach (var file in files)
      {
        var fileStruct = unpackTxnFile.Match(file.Path);
        Debug.Assert(fileStruct.Success);
        Debug.Assert(directory.Path.Equals(fileStruct.Groups[1].Value, StringComparison.Ordinal));
        var txn = GetRepairTransaction(int.Parse(fileStruct.Groups[3].Value), list);
        var fileName = fileStruct.Groups[2].Value;
        if (IsTransactionCommitFlag(fileName))
        {
          txn.SetCommitFile(file);
        } else
        {
          txn.CreateEnlistedFile(directory.File(fileName), directory, true);
        }
      }
    }
    isRepaired = true;
    return Task.WhenAll(list.Values.Select(i=>i.DoRepair()));
  }

  private static bool IsTransactionCommitFlag(string fileName)
  {
    return fileName.Equals(TransactedDirectory.CommitFlagName, StringComparison.Ordinal);
  }

  private RepairTransaction 
    GetRepairTransaction(int transactionNumber, Dictionary<int, RepairTransaction> list)
  {
    if (list.TryGetValue(transactionNumber, out var txn)) return txn;
    var ret = new RepairTransaction(transactionNumber);
    list.Add(transactionNumber, ret);
    return ret;
  }
  #endregion
}