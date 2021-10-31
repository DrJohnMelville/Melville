using System;
using System.Threading.Tasks;
using Melville.FileSystem.RelativeFiles;

namespace Melville.FileSystem.PseudoTransactedFS;

public interface ITransactedDirectory : IDirectory, IDisposable
{
  Task Commit();
}

public class PassthroughTransactedDirectory : DirectoryAdapterBase, ITransactedDirectory
{
  protected readonly IDirectory inner;
  public PassthroughTransactedDirectory(IDirectory inner)
  {
    this.inner = inner;
  }

  protected override IDirectory GetTargetDirectory() => inner;

  public void Dispose()
  {
    // Do nothing because we are just faking the transactions
  }
  public Task Commit()
  {
    // Do nothing because we are just faking the transactions
    return Task.CompletedTask;
  }
} 

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
  public async Task Commit()
  {
    var commitFile = await CreeateCommitFileIfNeeded();
    await transaction.Commit();// need to do the null commit to clear the DisposeCounter flag even if nothing
    // to commit.
    commitFile?.Delete();
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