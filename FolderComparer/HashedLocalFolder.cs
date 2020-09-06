using System;

namespace FolderComparer
{
    public class HashedLocalFolder
    {
        private readonly LocalFolder[] _innerFolders;
        private readonly LocalFile[] _files;
        private readonly Byte[] Hash;
    }
}