using Pipelines.Pipes;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

namespace FolderComparer
{
    public sealed class SingleThreadLocalFileBlocksReader : IPipeMiddleItem<IFile, IHashableFileBlock>
    {
        private const int BlockSize = 4096;
        private static readonly AutoResetEvent _resetEvent = new(true);
        public BlockingCollection<IHashableFileBlock> Output { get; }
        public BlockingCollection<IFile> Input { get; }

        public SingleThreadLocalFileBlocksReader(BlockingCollection<IFile> source, BlockingCollection<IHashableFileBlock> destination)
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

        private void ReadBlocks(IFile file)
        {
            FileInfo info = new FileInfo(file.Path, file.FileId, file.DirectoryId);
            using FileStream fileStream = File.OpenRead(file.Path);

            Int64 length = fileStream.Length;
            Int32 bufferSize = length < BlockSize ? (Int32)length : BlockSize;

            Int32 blockCount = 1;
            while (ReadBlock(fileStream, bufferSize, out Buffer? readedBlock))
            {
                FileBlock block;
                    if (fileStream.Position == length)
                    block = new FileBlock(readedBlock, blockCount, info, true);
                else
                    block = new FileBlock(readedBlock, blockCount, info, false);

                blockCount++;
                Output.Add(block);
            }
        }

        private static bool ReadBlock(Stream stream, Int32 bufferSize, [NotNullWhen(true)] out Buffer? buffer)
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

        private static Buffer GetBuffer(Int32 size) => new Buffer(ArrayPool<Byte>.Shared.Rent(size), size, true);

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
