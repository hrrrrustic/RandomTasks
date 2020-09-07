using System;
using System.Buffers;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;

namespace FolderComparer
{
    public sealed class FileBlock : IDisposable
    {
        private readonly Byte[] _block;

        public readonly FileInfo _info;
        public readonly Guid FileId;
        public readonly Int32 BlockNumber;
        public readonly Boolean IsLastBLock;

        public FileBlock(Byte[] block, Guid fileId, Int32 blockNumber, FileInfo info)
        {
            _block = block;
            FileId = fileId;
            BlockNumber = blockNumber;
            _info = info;
        }

        public HashedFileBlock HashBlock(HashAlgorithm hashAlgorithm)
        {
            Byte[] hash = hashAlgorithm.ComputeHash(_block);

            return new HashedFileBlock(this, hash, BlockNumber);
        }

        public void Dispose()
        {
            ArrayPool<Byte>.Shared.Return(_block);
        }
    }
}