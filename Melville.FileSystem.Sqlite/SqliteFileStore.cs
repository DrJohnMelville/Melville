using System.Data;
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
        BEGIN;
        INSERT INTO FsObjects 
        (Name, Parent, CreatedTime, LastWrite, Attributes, Length, BlockSize) VALUES 
        (@Name, @Parent, @CreatedTime, @LastWrite, @Attributes, @Length, @BlockSize);
        SELECT last_insert_rowid();
        COMMIT;
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
}

public class FSObject
{
    public required long Id { get; set; }
    public required string Name { get; init; }
    public required long? Parent { get; init; }
    public required long CreatedTime { get; init; }
    public required long LastWrite { get; init; }
    public required FileAttributes Attributes { get; init; }
    public required long Length { get; init; }
    public required long BlockSize { get; init; }
}

public static class DbTables
{
    public static readonly Migration[] All = [
        new(1,"""
        CREATE TABLE FsObjects (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Name STRING NOT NULL,
        Parent INTEGER,
        CreatedTime INTEGER NOT NULL,
        LastWrite INTEGER NOT NULL,
        Attributes INTEGER NOT NULL,
        Length INTEGER NOT NULL,
        BlockSize INTEGER NOT NULL,
        
        FOREIGN KEY(Parent) REFERENCES FsObjects(Id) ON DELETE CASCADE ON UPDATE CASCADE,
        UNIQUE (Name COLLATE NOCASE, Parent) ON CONFLICT REPLACE
        );
        """)
    ];
}