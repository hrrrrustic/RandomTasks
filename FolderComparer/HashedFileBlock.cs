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

        public HashedFileBlock(int blockNumber, Guid fileId, bool isLastBlock)
        {
            BlockNumber = blockNumber;
            FileId = fileId;
            IsLastBlock = isLastBlock;
        }
    }
}
