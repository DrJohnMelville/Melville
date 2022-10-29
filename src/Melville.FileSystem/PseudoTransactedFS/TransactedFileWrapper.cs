using System;
using Melville.FileSystem.WrappedDirectories;
using Melville.INPC;

namespace Melville.FileSystem.PseudoTransactedFS;

internal sealed partial class TransactedFileWrapper : IFileWrapper
{
    [FromConstructor]private readonly PseudoTransaction transaction;

    public IFile WrapFile(IFile source, IDirectory? wrappedDirectory) =>
        transaction.CreateEnlistedFile(source, 
            wrappedDirectory??throw new ArgumentException("Transacted files should have a parent."));

    public IDirectory WrapDirectory(IDirectory source, IDirectory wrappedParent) => 
        new WrappedDirectory(source, wrappedParent, this);
  
}