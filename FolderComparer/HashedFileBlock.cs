using System;
using System.Linq;

namespace FolderComparer
{
    public class HashedFileBlock
    {
        private readonly FileBlock _block;
        public readonly Byte[] _hash;
        public readonly FileInfo FileInfo;
        public readonly Int32 BlockNumber;

        public Guid FileId => _block.FileId;

        public HashedFileBlock(FileBlock block, Byte[] hash, Int32 blockNumber)
        {
            _block = block;
            _hash = hash;
            BlockNumber = blockNumber;
            FileInfo = _block._info;
        }

        public static Boolean operator ==(HashedFileBlock first, HashedFileBlock second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (first is null || second is null)
                return false;

            return first._hash.SequenceEqual(second._hash);
        }


        public static Boolean operator !=(HashedFileBlock first, HashedFileBlock second)
        {
            return !(first == second);
        }
    }
}