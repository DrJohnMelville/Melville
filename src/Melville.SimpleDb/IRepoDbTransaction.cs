namespace Melville.SimpleDb;

public interface IRepoDbTransaction: IRepoDbConnection
{
    void Commit();
    void Rollback();
}
