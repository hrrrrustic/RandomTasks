using System;
using System.Buffers;
using System.Runtime.ConstrainedExecution;

namespace FolderComparer
{
    public sealed class FileBlock : IDisposable
    {
        private readonly Byte[] _block;

        public readonly Boolean IsLastBLock;

        public FileBlock(Byte[] block)
        {
            _block = block;
        }

        public void Dispose()
        {
            ArrayPool<Byte>.Shared.Return(_block);
        }
    }
}