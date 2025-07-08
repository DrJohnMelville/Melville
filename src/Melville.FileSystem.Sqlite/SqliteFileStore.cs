using System.Data;
using System.Data.SQLite;
using Dapper;
using Melville.FileSystem.PseudoTransactedFS;
using Melville.SimpleDb;

namespace Melville.FileSystem.Sqlite;

public readonly struct SqliteFileStore(SqliteTransactionScope connection): IDisposable
{
    public SqliteFileStore(IRepoDbConnection connection, IDbTransaction? transaction = null) :
        this(new(connection, transaction))
    {
    }

    public IDirectory UntransactedRoot(string name)
    {
        var ret = new SqliteDirectory(this, name, name, 0);
        return TryCreateObject(ret, 0, true);
    }

    public static SqliteFileStore Create(string filePath = "")
    {
        var repo = new MigratedRepoConnection(
            new RepoDbConfiguration() { FolderPath = filePath }, DbTables.All);
        var dbConnection = repo.GetConnection();
        dbConnection.Execute("PRAGMA locking_mode = EXCLUSIVE;");
        return new(new SqliteTransactionScope(repo, null));
    }

    public FSObject CreateItem(string name, long parentDirectoryId, FileAttributes attributes,
        long blockSize)
    {
        var dto = CreateFsObject(name, parentDirectoryId, attributes, blockSize);
        dto.Id = connection.ExecuteScalar<long>("""
                                                INSERT INTO FsObjects 
                                                (Name, Parent, CreatedTime, LastWrite, Attributes, Length, BlockSize) VALUES 
                                                (@Name, @Parent, @CreatedTime, @LastWrite, @Attributes, @Length, @BlockSize)
                                                RETURNING Id;
                                                """, dto);
        return dto;
    }

    private static FSObject CreateFsObject(string name, long parentDirectoryId, FileAttributes attributes,
        long blockSize)
    {
        var now = DateTime.Now.Ticks;
        var dto = new FSObject()
        {
            Id = 0,
            Attributes = attributes,
            CreatedTime = now,
            LastWrite = now,
            BlockSize = blockSize,
            Length = 0,
            Name = name,
            Parent = parentDirectoryId < 1 ? null : parentDirectoryId,
        };
        return dto;
    }

    public T TryCreateObject<T>(T ret, long parentId, bool isDir) where T : SqliteFileSystemObject
    {
        if (FindObject(ret.Name, parentId) is { } fso &&
            BlockSizeMatchesType<T>(isDir, fso.BlockSize))
            ret.PopulateFrom(fso);
        return ret;
    }

    private static bool BlockSizeMatchesType<T>(bool isDir, long blockSize) where T :
        SqliteFileSystemObject => isDir ^ blockSize >= 0;

    public FSObject? FindObject(string name, long parentId) =>
        connection.QueryFirstOrDefault<FSObject>("""
                                                 SELECT Id, Name, Parent, CreatedTime, LastWrite, Attributes, Length, BlockSize FROM FsObjects 
                                                 WHERE (Parent IS @Parent OR @Parent IS NULL) AND Name = @Name COLLATE NOCASE
                                                 """,
            new { Name = name, Parent = parentId > 1 ? (long?)parentId : null });

    public IEnumerable<FSObject> SubDirectories(long parentKey)
    {
        return connection.Query<FSObject>("""
                                          SELECT Id, Name, Parent, CreatedTime, LastWrite, Attributes, Length, BlockSize FROM FsObjects 
                                          WHERE Parent IS @parentKey AND BlockSize < 0 
                                          """, new { parentKey });
    }

    public IEnumerable<FSObject> Files(long parentKey)
    {
        return connection.Query<FSObject>("""
                                          SELECT Id, Name, Parent, CreatedTime, LastWrite, Attributes, Length, BlockSize FROM FsObjects 
                                          WHERE Parent IS @parentKey AND BlockSize >= 0 
                                          """, new { parentKey });
    }

    public void DeleteItem(long objectId) =>
        connection.Execute("DELETE FROM FsObjects WHERE Id = @Id", new { Id = objectId });

    public FSObject? DirectoryById(long id)
    {
        return connection.QuerySingleOrDefault<FSObject>("""
                                                         SELECT Id, Name, Parent, CreatedTime, LastWrite, Attributes, Length, BlockSize FROM FsObjects 
                                                         WHERE Id = @id 
                                                         """, new { id });
    }

    public void DeleteBlocksFor(long fileId) =>
        connection.ExecuteAsync("""
                                DELETE FROM Blocks WHERE FileId = @fileId
                                """, new { fileId });

    public void UpdateFileData(long fileId, long length, long blockSize) =>
        connection.Execute("""
                           UPDATE FsObjects SET Length = @Length, LastWrite = @LastWrite, BlockSize = @blockSize 
                           WHERE Id = @Id
                           """, UpdateParameters(fileId, length, blockSize));

    private static object UpdateParameters(long fileId, long length, long blockSize) =>
        new { Length = length, LastWrite = DateTime.Now.Ticks, Id = fileId, blockSize };

    public SQLiteBlobWrapper GetBlobForWriting(
        long fileId, long blockSize, long blockIndex, SQLiteBlobWrapper blob)
    {
        var blockId = connection.ExecuteScalar<long>(
            """
            INSERT INTO Blocks (FileId, SequenceNumber, Bytes) 
            VALUES (@fileId, @blockIndex, zeroblob(@blockSize))
            ON CONFLICT(FileId, SequenceNumber) DO UPDATE SET Bytes = zeroblob(@blockSize)
            RETURNING Id
            """, new { fileId, blockSize, blockIndex });
        return CreateBlob(blockId, false, blob);
    }

    private SQLiteBlobWrapper CreateBlob(long blockId, bool readOnly, SQLiteBlobWrapper blob)
    {
        if (!blob.IsValid) return connection.BlobWrapper(blockId, readOnly);
        blob.Reopen(blockId);
        return blob;
    }

    public SQLiteBlobWrapper GetBlobForReading(long fileId, long sequence, SQLiteBlobWrapper blob)
    {
        var id = connection.ExecuteScalar<long>("""
                                                SELECT Id FROM Blocks 
                                                WHERE FileId = @fileId AND SequenceNumber = @sequence
                                                """, new { fileId, sequence });
        return CreateBlob(id, true, blob);
    }

    public async Task<SQLiteBlobWrapper> GetBlobForReadingAsync(
        long fileId, long sequence, SQLiteBlobWrapper blob)
    {
        var id = await connection.ExecuteScalarAsync<long>(
            """
            SELECT Id FROM Blocks 
            WHERE FileId = @fileId AND SequenceNumber = @sequence
            """, new { fileId, sequence });
        return CreateBlob(id, true, blob);
    }

    #region Transactions
    public ITransactedDirectory TransactedRoot(string name)
    {
        var ret = new SqliteTransactedDirectory(
            new SqliteFileStore(connection.NewTransactions()), name, name, 0);
        return TryCreateObject(ret, 0, true);
    }

    public void Commit() => connection.Commit();
    public void Rollback() => connection.Rollback();
    public void DisposeTransaction() => connection.DisposeTransaction();
    #endregion

    public void Dispose() => connection.Dispose();
}