using System;
using System.Linq;

namespace FolderComparer
{
    public class HashedFileBlock : IHashedFileBlock
    {
        public int BlockNumber { get; }
        public bool IsLastBlock { get; }
        public FileInfo FileInfo { get; }
        public byte[] Hash { get; }

        public HashedFileBlock(byte[] hash, int blockNumber, FileInfo fileInfo, bool isLastBlock)
        {
            Hash = hash;
            BlockNumber = blockNumber;
            FileInfo = fileInfo;
            IsLastBlock = isLastBlock;
        }

        public byte[] MergeHash(byte[] anotherBlock)
        {
            if (Hash.Length != anotherBlock.Length)
                throw new Exception();

            return Hash.Select((k, i) => (Byte)(k ^ anotherBlock[i])).ToArray();
        }
    }
}
