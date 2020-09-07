using System;
using System.Buffers;
using System.Security.Cryptography;
using FolderComparer.Files;

namespace FolderComparer.Blocks
{
    public sealed class FileBlock : IDisposable
    {
        private readonly Byte[] _block;
        public readonly FileInfo FileInfo;
        public readonly Int32 BlockNumber;

        public FileBlock(Byte[] block, Int32 blockNumber, FileInfo fileInfo)
        {
            _block = block;
            BlockNumber = blockNumber;
            FileInfo = fileInfo;
        }

        public HashedFileBlock HashBlock(HashAlgorithm hashAlgorithm)
        {
            Byte[] hash = hashAlgorithm.ComputeHash(_block);

            return HashedFileBlock.FromFileBlock(this, hash); // TODO : Где-то здесь надо диспоузить
        }

        public void Dispose()
        {
            ArrayPool<Byte>.Shared.Return(_block);
        }
    }
}