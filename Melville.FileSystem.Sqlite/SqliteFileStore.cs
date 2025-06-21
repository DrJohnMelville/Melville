using System.Data;
using System.Data.SQLite;
using System.Xml.Linq;
using Dapper;
using Melville.SimpleDb;
using Microsoft.VisualBasic;

namespace Melville.FileSystem.Sqlite;

public readonly struct SqliteFileStore(IDbConnection connection)
{
    public static async Task<SqliteFileStore> Create(string filePath = "")
    {
        var conn =
                new MigratedRepo(
                    new RepoConnection(new RepoDbConfiguration() { FolderPath = filePath }),
                    new Migrator(DbTables.All));
        return new(await conn.GetConnectionAsync());
    }

    private const string CreateFsObjectSql = """
        INSERT INTO FsObjects 
        (Name, Parent, CreatedTime, LastWrite, Attributes, Length, BlockSize) VALUES 
        (@Name, @Parent, @CreatedTime, @LastWrite, @Attributes, @Length, @BlockSize)
        RETURNING Id;
        """;

    public FSObject CreateItem(string name, long parentDirectoryId, FileAttributes attributes,
        long blockSize)
    {
        var dto = CreateFsObject(name, parentDirectoryId, attributes, blockSize);
        dto.Id = connection.ExecuteScalar<long>(CreateFsObjectSql, dto);
        return dto;
    }
    public async Task<FSObject> CreateItemAsync(
        string name, long parentDirectoryId, FileAttributes attributes, long blockSize)
    {
        var dto = CreateFsObject(name, parentDirectoryId, attributes, blockSize);
        dto.Id = await connection.ExecuteScalarAsync<long>(CreateFsObjectSql, dto);
        return dto;
    }

    private static FSObject CreateFsObject(string name, long parentDirectoryId, FileAttributes attributes, long blockSize)
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
            Parent = parentDirectoryId < 1?null:parentDirectoryId,
        };
        return dto;
    }

    public FSObject? FindObject(string name, long parentId) =>
        connection.QueryFirstOrDefault<FSObject>("""
            SELECT Id, Name, Parent, CreatedTime, LastWrite, Attributes, Length, BlockSize FROM FsObjects 
            WHERE (Parent IS @Parent OR @Parent IS NULL) AND Name = @Name COLLATE NOCASE
            """, new { Name = name, Parent = parentId > 1 ? (long?)parentId : null });

    public IEnumerable<FSObject> SubDirectories(long parentKey)
    {
        return connection.Query<FSObject>("""
            SELECT Id, Name, Parent, CreatedTime, LastWrite, Attributes, Length, BlockSize FROM FsObjects 
            WHERE Parent IS @parentKey AND BlockSize < 0 
            """, new { parentKey});

    }

    public IEnumerable<FSObject> Files(long parentKey)
    {
        return connection.Query<FSObject>("""
            SELECT Id, Name, Parent, CreatedTime, LastWrite, Attributes, Length, BlockSize FROM FsObjects 
            WHERE Parent IS @parentKey AND BlockSize >= 0 
            """, new { parentKey});

    }

    public void DeleteItem(long objectId) => 
        connection.Execute("DELETE FROM FsObjects WHERE Id = @Id", new { Id = objectId });

    public FSObject? DirectoryById(long id)
    {
        return connection.QuerySingleOrDefault<FSObject>("""
        SELECT Id, Name, Parent, CreatedTime, LastWrite, Attributes, Length, BlockSize FROM FsObjects 
        WHERE Id = @id 
        """, new {id});
    }

    public Task DeleteBlocksForAsync(long fileId) =>
        connection.ExecuteAsync("""
            DELETE FROM Blocks WHERE FileId = @fileId
            """, new { fileId });

    public void UpdateFileData(long fileId, long length) => 
        connection.Execute(UpdateFileQuery, UpdateParameters(fileId, length));

    public Task UpdateFileDataAsync(long fileId, long length) => 
        connection.ExecuteAsync(UpdateFileQuery, UpdateParameters(fileId, length));

    private const string UpdateFileQuery = """
        UPDATE FsObjects SET Length = @Length, LastWrite = @LastWrite 
        WHERE Id = @Id
        """;
    private static object UpdateParameters(long fileId, long length) => 
        new { Length = length, LastWrite = DateTime.Now.Ticks, Id = fileId };

    private const string GetWriteBlockQuery = """
        INSERT INTO Blocks (FileId, SequenceNumber, Bytes) 
        VALUES (@fileId, @blockIndex, zeroblob(@blockSize))
        ON CONFLICT(FileId, SequenceNumber) DO UPDATE SET Bytes = zeroblob(@blockSize)
        RETURNING Id
        """;
    
    public SQLiteBlobWrapper GetBlobForWriting(
        long fileId, long blockSize, long blockIndex, SQLiteBlobWrapper blob)
    {
        var blockId = connection.ExecuteScalar<long>(
            GetWriteBlockQuery, new { fileId, blockSize,blockIndex });
        return CreateBlob(blockId, false, blob);
    }

    public async Task<SQLiteBlobWrapper> GetBlobForWritingAsync(
        long fileId, long blockSize, long blockIndex, SQLiteBlobWrapper blob)
    {
        var blockId = await connection.ExecuteScalarAsync<long>(
            GetWriteBlockQuery, new { fileId, blockSize,blockIndex });
        return CreateBlob(blockId, false, blob);
    }

    private SQLiteBlobWrapper CreateBlob(long blockId, bool readOnly, SQLiteBlobWrapper blob)
    {
        if (!blob.IsValid)
        return new SQLiteBlobWrapper(SQLiteBlob.Create((SQLiteConnection)connection,
                "main", "Blocks", "Bytes", blockId, readOnly)); 
        blob.Reopen(blockId);
        return blob;
    }

    private const string GetReadBlockQuery = """
        SELECT Id FROM Blocks 
        WHERE FileId = @fileId AND SequenceNumber = @sequence
        """;

    public SQLiteBlobWrapper GetBlobForReading(long fileId, long sequence, SQLiteBlobWrapper blob)
    {
        var id = connection.ExecuteScalar<long>(GetReadBlockQuery, new { fileId, sequence });
        return CreateBlob(id, true, blob);
    }

    public async Task<SQLiteBlobWrapper> GetBlobForReadingAsync(
        long fileId, long sequence, SQLiteBlobWrapper blob)
    {
        var id = await connection.ExecuteScalarAsync<long>(
            GetReadBlockQuery, new { fileId, sequence });
        return CreateBlob(id, true, blob);
    }
}