using  System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Xunit;

namespace Melville.TestHelpers.StringDatabase;

public sealed class StringTestDatabase : IDisposable
{

  #region Load And Store

  private StringTestDatabaseHolder? stringData;
  private StringTestDatabaseHolder TryLoadStore(string? sourceFileName)
  {
    if (sourceFileName == null) throw new ArgumentNullException(nameof(sourceFileName));
    return stringData ??= new StringTestDatabaseHolder(sourceFileName);
  }

  public void Dispose() => stringData?.Dispose();

  #endregion


  private string ComputeDatumKey(string member, string? file)
  {
    var datumKey = $"{Path.GetFileName(file??"NullFile.Txt")}_{member}";
    return datumKey;
  }

  /// <summary>
  /// Test a result against the database.  If in recording mode, record the answer instead.
  /// </summary>
  /// <param name="result">The result to be checked.</param>
  /// <param name="member">Name of the test, or if null the name of the calling method.</param>
  /// <param name="file">Leave blank so the file name will pass through</param>
  public void AssertDatabase(string result, [CallerMemberName] string member = "",
    [CallerFilePath] string? file = null)
  {
    if (Recording)
    {
      RecordDatabase(result, member, file);
    }
    else
    {
      Assert.Equal(TryLoadStore(file).LookupResult(ComputeDatumKey(member, file)), result);
    }
  }
  /// <summary>
  /// Test the hash of a byte array against the database
  /// </summary>
  /// <param name="result">The result to be checked.</param>
  /// <param name="member">Name of the test, or if null the name of the calling method.</param>
  /// <param name="file">Leave blank so the file name will pass through</param>
  public void AssertDatabase(byte[] result, [CallerMemberName] string member = "",
    [CallerFilePath] string? file = null)
  {
    var hasher = MD5.Create();
    hasher.ComputeHash(result);
    AssertDatabase(Convert.ToBase64String(hasher.Hash), member, file);
  }

  /// <summary>
  /// Record a new result in the database
  /// </summary>
  /// <param name="result">The result to be recorded.</param>
  /// <param name="member">Name of the test, or if null the name of the calling method.</param>
  /// <param name="file">Leave blank so the file name will pass through</param>
  public void RecordDatabase(string result, [CallerMemberName] string member = "",
    [CallerFilePath] string? file = null)
  {
    TryLoadStore(file).WriteResult(ComputeDatumKey(member, file), result);
  }

  public bool Recording { get; set; }
    
  #region AssertStream
  /// <summary>
  /// Returns a writable stream.  When the stream is disposed, assert the result against this database.
  /// </summary>
  /// <param name="member">Member where this item was called from (or name of the test)</param>
  /// <param name="file">File path where the code under test is located</param>
  /// <returns>An asserting stream.  </returns>
  public Stream AssertStream([CallerMemberName] string member = "",
    [CallerFilePath] string? file = null) => new AssertingStream(this, member, file);

  private class AssertingStream : MemoryStream
  {
    private StringTestDatabase db;
    private readonly string itemName;
    private readonly string? fileName;
    public AssertingStream(StringTestDatabase db, string itemName, string? fileName)
    {
      this.db = db;
      this.itemName = itemName;
      this.fileName = fileName;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.AssertDatabase(ToArray(), itemName, fileName);
      }
      base.Dispose(disposing);
    }
  }
  #endregion
}