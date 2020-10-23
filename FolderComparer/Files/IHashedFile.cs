using System;

namespace FolderComparer
{
    public interface IHashedFile
    {
        byte[] Hash { get; }
        Guid FolderId { get; }
    }
}
