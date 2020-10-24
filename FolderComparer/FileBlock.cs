using System;
using System.Security.Cryptography;

namespace FolderComparer
{
    public sealed class FileBlock : IHashableFileBlock, IDisposable
    {
        private readonly Buffer _block;
        public FileInfo FileInfo { get; }
        public Int32 BlockNumber { get; }
        public bool IsLastBlock { get; }

        public FileBlock(Buffer block, Int32 blockNumber, FileInfo fileInfo, bool isLastBlock)
        {
            _block = block;
            BlockNumber = blockNumber;
            FileInfo = fileInfo;
            IsLastBlock = isLastBlock;
        }

        public IHashedFileBlock HashBlock(HashAlgorithm algorithm)
        {
            var hash = algorithm.ComputeHash(_block.ByteBuffer);
            var hashedBLock = new HashedFileBlock(hash, BlockNumber, FileInfo, IsLastBlock);
            Dispose();
            return hashedBLock;
        }

        public void Dispose()
        {
            _block.Dispose();
        }
    }
}
