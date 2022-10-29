using System.Threading.Tasks;
using Melville.FileSystem;
using Melville.FileSystem.PseudoTransactedFS;

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