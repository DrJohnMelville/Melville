using System.Data;

namespace Melville.SimpleDb;

public interface IRepoDbConnection
{
    IDbConnection GetConnection();
}