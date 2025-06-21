using Melville.SimpleDb;

namespace Melville.FileSystem.Sqlite;

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

            CREATE TABLE Blocks (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            FileId INTEGER NOT NULL,
            SequenceNumber INTEGER NOT NULL,
            Bytes BLOB NOT NULL,
            UNIQUE (FileId, SequenceNumber),
            FOREIGN KEY (FileId) REFERENCES FsObjects(Id) ON DELETE CASCADE ON UPDATE CASCADE
            );
            """)
    ];
}