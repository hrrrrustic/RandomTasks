using System;

namespace FolderComparer
{
    public class HashedLocalFile
    {
        public readonly FileInfo File;
        public readonly Byte[] Hash;

        public HashedLocalFile(FileInfo file, Byte[] hash)
        {
            File = file;
            Hash = hash;
        }

        public Guid FolderId => File.FolderId;
    }
}