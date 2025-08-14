using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Melville.FileSystem.WrappedDirectories;

namespace Melville.FileSystem.PseudoTransactedFS;

internal class PseudoTransaction: ITransactionControl
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

    public IFile CreateEnlistedFile(IFile untransactedFile, IDirectory parentDir, bool createdFromRecovery)
    {
        var untransactedDirectory = untransactedFile?.Directory ??
            throw new ArgumentException("files enlisted in a transaction mush have a parent directory");
        var ret = new TransactedFile(untransactedFile,
            CreateShadowFile(untransactedFile, untransactedDirectory, createdFromRecovery),
            parentDir);
        if (HasExistingFile(ret, out var file)) return file;
        items.Add(ret);
        return ret;
    }

    private Lazy<IFile> CreateShadowFile(IFile untransactedFile, IDirectory untransactedDirectory,
        bool createdFromRecovery)
    {
        var shadowFile = new Lazy<IFile>(()=>
            untransactedDirectory.File($"{untransactedFile.Name}.{transactionNumber}.txn"));
        if (createdFromRecovery)
            _ = shadowFile.Value
                ;        return shadowFile;
    }

    private bool HasExistingFile(IFile candidate, [NotNullWhen(true)] out IFile? file)
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

    public ValueTask Commit()
    {
        if (items.Count == 0) return new ValueTask();
        var commitTasks = ItemsToCommit().Select(i => i.Commit()).ToList();
        items.Clear();
        return new(Task.WhenAll(commitTasks));
    }

    private IEnumerable<TransactedFile> ItemsToCommit() => items.Where(i => i.HasPendingCommit());

    public bool HasItemsToCommit() => ItemsToCommit().Any();

    public void Rollback()
    {
        foreach (var transactedFile in items)
        {
            transactedFile.Rollback();
        }

        items.Clear();
    }
}