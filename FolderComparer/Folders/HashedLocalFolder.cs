using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FolderComparer.Files;

namespace FolderComparer.Folders
{
    public class HashedLocalFolder : IEnumerable<HashedLocalFile>
    {
        public readonly HashedLocalFile[] HashedFiles;
        public readonly Byte[] Hash;

        public HashedLocalFolder(HashedLocalFile[] hashedFiles, Byte[] hash)
        {
            HashedFiles = hashedFiles;
            Hash = hash;
        }

        #region ObjectOverride
        public Boolean Equals(HashedLocalFolder other)
        {
            if (other is null)
                return false;

            return Hash.SequenceEqual(other.Hash);
        }

        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is HashedLocalFolder hashedFolder && Equals(hashedFolder);
        }

     

        public override Int32 GetHashCode()
        {
            return HashCode.Combine(HashedFiles, Hash);
        }


        #endregion

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<HashedLocalFile> GetEnumerator() => HashedFiles.AsEnumerable().GetEnumerator();
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