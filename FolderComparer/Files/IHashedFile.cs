using System;

namespace FolderComparer
{
    public interface IHashedFile : IFile, IEquatable<IHashedFile>
    {
        byte[] Hash { get; }
        byte[] MergeHash(byte[] anotherFile);
    }
}
