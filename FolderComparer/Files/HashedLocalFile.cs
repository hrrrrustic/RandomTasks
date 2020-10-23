using System;

namespace FolderComparer.Files
{
    public class HashedLocalFile : IHashedFile
    {
        public byte[] Hash { get; }

        public Guid FolderId { get; }

        public HashedLocalFile(byte[] hash, Guid folderId)
        {
            Hash = hash;
            FolderId = folderId;
        }
    }
}
