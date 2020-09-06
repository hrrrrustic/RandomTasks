using System;
using System.Buffers;
using System.Runtime.ConstrainedExecution;

namespace FolderComparer
{
    public sealed class FileBlock : IDisposable
    {
        private readonly Byte[] _block;

        private readonly Guid _fileId;

        private readonly Guid _folderId;
        public readonly Boolean IsLastBLock;

        public FileBlock(Byte[] block, Guid folderId, Guid fileId)
        {
            _block = block;
            _folderId = folderId;
            _fileId = fileId;
        }

        public void Dispose()
        {
            ArrayPool<Byte>.Shared.Return(_block);
        }
    }
}