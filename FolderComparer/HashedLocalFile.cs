using System;

namespace FolderComparer
{
    public class HashedLocalFile
    {
        private readonly LocalFile _file;
        private readonly Byte[] Hash;

        public HashedLocalFile(LocalFile file, Byte[] hash)
        {
            _file = file;
            Hash = hash;
        }
    }
}