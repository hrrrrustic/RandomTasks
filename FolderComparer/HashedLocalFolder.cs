using System;
using System.Linq;

namespace FolderComparer
{
    public class HashedLocalFolder
    {
        public readonly HashedLocalFile[] HashedFiles;
        public readonly Byte[] Hash;

        public HashedLocalFolder(HashedLocalFile[] hashedFiles, Byte[] hash)
        {
            HashedFiles = hashedFiles;
            Hash = hash;
        }

        public static Boolean operator ==(HashedLocalFolder first, HashedLocalFolder second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (first is null || second is null)
                return false;

            return first.Hash.SequenceEqual(second.Hash);
        }

        public static Boolean operator !=(HashedLocalFolder first, HashedLocalFolder second)
        {
            return !(first == second);
        }
    }
}