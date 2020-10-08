using System;
using System.Buffers;
using System.Linq;
using System.Security.Cryptography;

namespace FolderComparer.Blocks
{
    public sealed class FileBlock : IDisposable
    {
        private readonly Buffer _block;
        public readonly LocalFileInfo LocalFileInfo;
        public readonly Int32 BlockNumber;
        private Boolean _disposed = false;

        public FileBlock(Buffer block, Int32 blockNumber, LocalFileInfo localFileInfo) => (_block, BlockNumber, LocalFileInfo) = (block, blockNumber, localFileInfo);

        public HashedFileBlock HashBlock(HashAlgorithm hashAlgorithm)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(FileBlock));

            Byte[] hash = hashAlgorithm.ComputeHash(_block.ByteBuffer, 0, _block.ActualSize);

            return HashedFileBlock.FromFileBlock(this, hash);
        }

        public void Dispose() => _block?.Dispose();
    }
}