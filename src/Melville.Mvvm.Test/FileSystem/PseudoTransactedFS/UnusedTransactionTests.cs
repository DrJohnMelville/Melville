using System.Threading.Tasks;
using Melville.FileSystem;
using Melville.FileSystem.PseudoTransactedFS;
using Moq;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.PseudoTransactedFS;

public class UnusedTransactionTests
{
    Mock<IDirectory> inner = new(MockBehavior.Strict);
    private TransactedDirectory sut2;

    public UnusedTransactionTests()
    {
        sut2 = new TransactedDirectory(inner.Object);
    }

    [Fact]
    public async Task NonWritingTransactionDoesNotTouchDiskOnCommit()
    {
        await sut2.Commit();
        inner.VerifyNoOtherCalls();
    }
    [Fact]
    public void NonWritingTransactionDoesNotTouchDiskOnRollback()
    {
        sut2.Rollback();
        inner.VerifyNoOtherCalls();
    }

}