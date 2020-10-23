using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class HashedFileBlock : IHashedFileBlock
    {
        public int BlockNumber { get; }

        public bool IsLastBlock { get; }

        public Guid FileId { get; }

        public byte[] Hash { get; }

        public HashedFileBlock(byte[] hash, int blockNumber, Guid fileId, bool isLastBlock)
        {
            Hash = hash;
            BlockNumber = blockNumber;
            FileId = fileId;
            IsLastBlock = isLastBlock;
        }

        public IHashedFileBlock MergeHash(IHashedFileBlock anotherBlock)
        {
            throw new NotImplementedException();
        }

        public byte[] MergeHash(byte[] anotherBlock)
        {
            throw new NotImplementedException();
        }
    }
}
