using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.PseudoTransactedFS;

public partial class TransactedDirectory
{
  public sealed partial class TransactedFile
  {
    internal class PseudoTransaction
    {
      private readonly List<TransactedFile> items = new();
      private static int transactionNumberUsed;
      private readonly int transactionNumber;
      public int TransactionNumber => transactionNumber;

      public PseudoTransaction()
        : this(Interlocked.Increment(ref transactionNumberUsed))
      {
      }

      protected PseudoTransaction(int transactionNumber)
      {
        this.transactionNumber = transactionNumber;
      }

      public IFile CreateEnlistedFile(IDirectory untransactedDirectory, string name)
      {
        var ret = new TransactedFile(untransactedDirectory.File(name), 
          untransactedDirectory.File($"{name}.{transactionNumber}.txn"), untransactedDirectory);
        if (HasExistingFile(ret, out var file)) return file;
        items.Add(ret);
        return ret;
      }

      private bool HasExistingFile(IFile candidate, [NotNullWhen(true)]out IFile? file)
      {
        file = items.FirstOrDefault(i => PathMatches(candidate, i));
        EnsureTransactionFileIsUnique(candidate, file);
        return file != null;
      }
      
      [Conditional("DEBUG")]
      private void EnsureTransactionFileIsUnique(IFile candidate, IFile? file)
      {
        Debug.Assert(!items.Any(i => PathMatches(candidate, i) && i != file));
      }

      private static bool PathMatches(IFile a, IFile b) => 
        b.Path.Equals(a.Path, StringComparison.OrdinalIgnoreCase);

      public Task Commit()
      {
        var commitTasks = ItemsToCommit().
          Select(i =>i.Commit()).
          ToArray();
        items.Clear();
        return Task.WhenAll(commitTasks);
      }
      private IEnumerable<TransactedFile> ItemsToCommit() => items.Where(i => i.innerShadow.Exists());

      public bool HasItemsToCommit() => ItemsToCommit().Any();

      public void Rollback()
      {
        foreach (var transactedFile in items)
        {
          transactedFile.innerShadow.Delete();
        }
        items.Clear();
      }
    }

    internal sealed class RepairTransaction: PseudoTransaction
    {
      private IFile? commitFile;
      public RepairTransaction(int transactionNumber) : base(transactionNumber)
      {
      }
      public void SetCommitFile(IFile file)
      {
        commitFile = file;
      }
      public async Task DoRepair()
      {
        if (commitFile == null)
        {
          Rollback();
        }
        else
        {
          await Commit();
          commitFile.Delete();
        }
      }
    }
    public byte FinalProgress => inner.FinalProgress;
    public Task WaitForFinal => inner.WaitForFinal;
  }
}