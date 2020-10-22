using Pipelines.Pipes;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class SingleThreadLocalFileBlocksReader : IPipeMiddleItem<ILocalFile, IHashableFileBlock>
    {
        private const int BlockSize = 4096;
        private static readonly AutoResetEvent _resetEvent = new(true);

        public BlockingCollection<IHashableFileBlock> Output { get; }

        public BlockingCollection<ILocalFile> Input { get; }

        public SingleThreadLocalFileBlocksReader(BlockingCollection<ILocalFile> source, BlockingCollection<IHashableFileBlock> destination)
        {
            Input = source;
            Output = destination;
        }

        public void StartReading()
        {
            _resetEvent.WaitOne();
            Reading();
            _resetEvent.Set();
        }

        private void Reading()
        {
            foreach (var localFile in Input.GetConsumingEnumerable())
            {
                ReadBlocks(localFile);
            }
        }

        private void ReadBlocks(ILocalFile file)
        {
            Guid fileId = Guid.NewGuid();
            using FileStream fileStream = File.OpenRead(file.Path);

            Int64 length = fileStream.Length;
            Int32 bufferSize = length < BlockSize ? (Int32)length : BlockSize;

            Int32 blockCount = 1;
            while (ReadBlock(fileStream, bufferSize, out Buffer? readedBlock))
            {
                FileBlock block = new FileBlock(readedBlock, blockCount, fileId, false);
                blockCount++;

                Output.Add(block);
            }
        }

        private bool ReadBlock(Stream stream, Int32 bufferSize, [NotNullWhen(true)] out Buffer? buffer)
        {
            buffer = GetBuffer(bufferSize);
            Int32 readedCount = stream.Read(buffer.ByteBuffer, 0, bufferSize);

            if (readedCount <= 0)
            {
                buffer.Dispose();
                buffer = null;
                return false;
            }

            if (readedCount != bufferSize)
                buffer.UpdateActualSize(readedCount);

            return true;
        }

        private Buffer GetBuffer(Int32 size) => new Buffer(ArrayPool<Byte>.Shared.Rent(size), size, true);

        public void Execute()
        {
            StartReading();
            Output.CompleteAdding();
        }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
        }
    }
}
