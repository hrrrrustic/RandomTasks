using System;
using System.Linq;
using FolderComparer.Files;

namespace FolderComparer.Blocks
{
    public class HashedFileBlock
    {
        public readonly Byte[] Hash;
        public readonly Int32 BlockNumber;
        public readonly LocalFileInfo LocalFileInfo;

        private HashedFileBlock(Byte[] hash, LocalFileInfo localFileInfo, Int32 blockNumber) 
            => (Hash, LocalFileInfo, BlockNumber) = (hash, localFileInfo, blockNumber);
        public static HashedFileBlock FromFileBlock(FileBlock block, Byte[] hash) 
            => new HashedFileBlock(hash, block.LocalFileInfo, block.BlockNumber);

        #region ObjectOverride
        public Boolean Equals(HashedFileBlock other)
        {
            if (other is null)
                return false;

            return Hash.SequenceEqual(other.Hash);
        }

        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is HashedFileBlock hashedBlock && Equals(hashedBlock);
        }

        public override Int32 GetHashCode()
        {
            return HashCode.Combine(Hash, LocalFileInfo);
        }
        #endregion

        public static Boolean operator ==(HashedFileBlock first, HashedFileBlock second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (first is null || second is null)
                return false;

            return first.Hash.SequenceEqual(second.Hash);
        }

        public static Boolean operator !=(HashedFileBlock first, HashedFileBlock second) => !(first == second);
    }
}