using System.Data;
using System.Threading.Tasks;

namespace Melville.SimpleDb;

public interface IRepoDbConnection
{
    ValueTask<IDbConnection> GetConnectionAsync();
}