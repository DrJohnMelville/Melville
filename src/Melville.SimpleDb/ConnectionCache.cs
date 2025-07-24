using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Melville.SimpleDb;

public interface IRepoConnectionFactory: IDisposable
{
    IRepoDbConnection Create();
    IRepoDbConnection CreateReadOnly();
}

