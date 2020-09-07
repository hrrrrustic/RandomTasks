using System;

namespace FolderComparer.Files
{
    public class HashedLocalFile
    {
        public readonly LocalFileInfo LocalFile;
        public readonly Byte[] Hash;

        public HashedLocalFile(LocalFileInfo localFile, Byte[] hash)
        {
            LocalFile = localFile;
            Hash = hash;
        }

        public Guid FolderId => LocalFile.FolderId;
    }
}