﻿using System;
using System.Buffers;

namespace FolderComparer
{
    public sealed class Buffer : IDisposable
    {
        public readonly Byte[] ByteBuffer;
        public Int32 ActualSize { get; private set; }
        private readonly Boolean _isArrayPoolBuffer;
        private Boolean _disposed;
        public Buffer(Byte[] buffer, Int32 actualSize, Boolean isArrayPoolBuffer)
            => (ByteBuffer, ActualSize, _isArrayPoolBuffer) = (buffer, actualSize, isArrayPoolBuffer);

        public void UpdateActualSize(Int32 newActualSize)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Buffer));

            if (newActualSize < 0 || newActualSize > ByteBuffer.Length)
                throw new ArgumentException($"{nameof(newActualSize)} must be non-negative and not grater than buffer size ({ByteBuffer.Length})");

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
