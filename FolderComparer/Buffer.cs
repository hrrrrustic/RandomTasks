using System;
using System.Buffers;

namespace FolderComparer
{
    public class Buffer : IDisposable
    {
        public readonly Byte[] ByteBuffer;
        public Int32 ActualSize { get; private set; }
        private readonly Boolean _isArrayPoolBuffer;
        private Boolean _disposed = false;
        public Buffer(Byte[] buffer, Int32 actualSize, Boolean isArrayPoolBuffer)
            => (ByteBuffer, ActualSize, _isArrayPoolBuffer) = (buffer, actualSize, isArrayPoolBuffer);

        public void UpdateActualSize(Int32 newActualSize)
        {
            if (newActualSize < 0 || newActualSize > ByteBuffer.Length)
                throw new ArgumentException();

            ActualSize = newActualSize;
        }
        public void Dispose()
        {
            if (_disposed)
                return;

            if (_isArrayPoolBuffer)
            {
                ArrayPool<Byte>.Shared.Return(ByteBuffer);
                _disposed = true;
            }
        }
    }
}
