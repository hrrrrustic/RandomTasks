using FolderComparer.Old;
using System;
using System.Security.Cryptography;

namespace FolderComparer
{
    public class FileBlock : IHashableFileBlock, IDisposable
    {
        private readonly Buffer _block;
        public readonly LocalFileInfo LocalFileInfo;
        public Int32 BlockNumber { get; }
        public Guid FileId { get; }

        public bool IsLastBlock { get; }

        public FileBlock(Buffer block, Int32 blockNumber, Guid fileId, bool isLastBlock)
        {
            _block = block;
            BlockNumber = blockNumber;
            FileId = fileId;
            IsLastBlock = isLastBlock;
        }

        public IHashedFileBlock HashBlock(HashAlgorithm algorithm)
        {
            var hash = algorithm.ComputeHash(_block.ByteBuffer);
            var hashedBLock = new HashedFileBlock(hash, BlockNumber, FileId, IsLastBlock);
            Dispose();
            return hashedBLock;
        }

        public void Dispose()
        {
            _block.Dispose();
        }
    }
}
