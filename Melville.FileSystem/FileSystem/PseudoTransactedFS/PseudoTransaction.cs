using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.FileSystem.PseudoTransactedFS
{
  public partial class TransactedDirectory
  {
    public sealed partial class TransactedFile
    {
      internal class PseudoTransaction
      {
        private readonly List<TransactedFile> items = new List<TransactedFile>();
        private static int transactionNumberUsed;
        private readonly int transactionNumber;
        public int TransactionNumber
        {
          get { return transactionNumber; }
        }

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
          var ret = new TransactedFile(untransactedDirectory.File(name), untransactedDirectory.
            File(name + "." + transactionNumber.ToString() + ".txn"), untransactedDirectory);
          var existingTransactedFile = items.FirstOrDefault(i => i.Path.Equals(ret.Path, StringComparison.Ordinal));
          Debug.Assert(!items.Any(i => i.Path.Equals(ret.Path, StringComparison.Ordinal) 
            && i != existingTransactedFile));
          if (existingTransactedFile != null) return existingTransactedFile;
          items.Add(ret);
          return ret;
        }

        public Task Commit()
        {
          var commitTasks = ItemsToCommit().
            Select(i =>i.Commit()).
            ToArray();
          items.Clear();
          return Task.WhenAll(commitTasks);
        }
        private IEnumerable<TransactedFile> ItemsToCommit()
        {
          return items.
            Where(i => i.innerShadow.Exists());
        }
        public bool HasItemsToCommit()
        {
          return ItemsToCommit().Any();
        }

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
}