using System;
using System.Threading.Tasks;
using Melville.FileSystem.RelativeFiles;
using Melville.FileSystem.WrappedDirectories;
using Melville.INPC;

namespace Melville.FileSystem.PseudoTransactedFS;

public interface ITransactionControl
{
  ValueTask Commit();
  void Rollback();
}
public interface ITransactedDirectory : IDirectory, ITransactionControl, IDisposable
{
}

public sealed partial class TransactedDirectory : ITransactedDirectory
{
  public const string CommitFlagName = "Committed";
  private readonly IDirectory innerDirectory;
  private readonly PseudoTransaction transaction;
  [DelegateTo]private readonly IDirectory wrappedDirectory;

  public TransactedDirectory(IDirectory innerDirectory)
  {
    this.innerDirectory = innerDirectory;
    transaction = new();
    wrappedDirectory = innerDirectory.WrapWith(new TransactedFileWrapper(transaction));
  }
  public void Dispose() => Rollback();
  public void Rollback() => transaction.Rollback();

  public async ValueTask Commit()
  {
    var commitFlag = await CreeteCommitFileIfNeeded();
    await transaction.Commit();
    commitFlag?.Delete();
  }

  private ValueTask<IFile?> CreeteCommitFileIfNeeded() => 
      transaction.HasItemsToCommit() ? CreateCommitFile() : new((IFile?)null);

    private async ValueTask<IFile?> CreateCommitFile()
    {
      var commitFile = innerDirectory.File($"{CommitFlagName}.{transaction.TransactionNumber}.txn");
      await using var stream = await commitFile.CreateWrite();
      return commitFile;
    }
}
