using System;
using System.Threading.Tasks;
using Melville.FileSystem.RelativeFiles;
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

public partial class PassthroughTransactedDirectory : ITransactedDirectory
{
  [FromConstructor][DelegateTo]protected readonly IDirectory inner;

  public void Dispose() { }

  public ValueTask Commit() => new();

  public void Rollback() {}
} 

#warning -- Make this a wrappedDirectory
public sealed partial class TransactedDirectory : DirectoryAdapterBase, ITransactedDirectory
{
  private readonly IDirectory inner;
  private readonly TransactedFile.PseudoTransaction transaction;
  public TransactedDirectory(IDirectory inner) :
    this(inner, new TransactedFile.PseudoTransaction())
  {
  }
  private TransactedDirectory(IDirectory inner, TransactedFile.PseudoTransaction transaction)
  {
    this.inner = inner;
    this.transaction = transaction;
  }

  protected override IDirectory GetTargetDirectory() => inner;
    
  public override IDirectory SubDirectory(string name) => 
    new TransactedDirectory(base.SubDirectory(name), transaction);

  public override IFile File(string name) => transaction.CreateEnlistedFile(inner, name);

  // if we have already committed than Rollback is a no-op anyway, no need to test for it
  public void Dispose()=> transaction.Rollback();
    
  public const string CommitFlagName = "Committed";
  public async ValueTask Commit()
  {
    var commitFile = await CreeateCommitFileIfNeeded();
    await transaction.Commit();// need to do the null commit to clear the DisposeCounter flag even if nothing
    // to commit.
    commitFile?.Delete();
  }

  public void Rollback()
  {
    transaction.Rollback();
  }

  private async Task<IFile?> CreeateCommitFileIfNeeded() => 
    transaction.HasItemsToCommit() ? await CreateCommitFile() : null;

  private async Task<IFile> CreateCommitFile()
  {
    var commitFile = inner.File($"{CommitFlagName}.{transaction.TransactionNumber}.txn");
    using (var stream = await commitFile.CreateWrite())
    {
      await stream.WriteAsync(new byte[] {32});
    }
    return commitFile;
  }
}