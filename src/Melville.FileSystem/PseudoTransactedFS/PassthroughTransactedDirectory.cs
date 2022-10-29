using System.Threading.Tasks;
using Melville.INPC;

namespace Melville.FileSystem.PseudoTransactedFS;

public partial class PassthroughTransactedDirectory : ITransactedDirectory
{
    [FromConstructor][DelegateTo]protected readonly IDirectory inner;

    public void Dispose() { }

    public ValueTask Commit() => new();

    public void Rollback() {}
}