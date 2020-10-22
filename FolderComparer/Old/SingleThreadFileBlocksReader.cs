﻿using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FolderComparer.Files;
using FolderComparer.Old.Blocks;
using FolderComparer.Old.Files;
using FolderComparer.Tools;

namespace FolderComparer
{
    public sealed class SingleThreadFileBlocksReader : IDisposable
    {
        private const Int32 BlockSize = 4096;
        private readonly List<Task> _blockPushingTasks = new();
        private static readonly AutoResetEvent _resetEvent = new(true);
        public readonly BlockingCollection<FileBlock> ReadedBlocks = new(new ConcurrentQueue<FileBlock>());
        public readonly IReadOnlyCollection<LocalFile2> QueuedFiles;
        public ReadingState Status { get; private set; } = ReadingState.NotStarted;

        public SingleThreadFileBlocksReader(IReadOnlyCollection<LocalFile2> queuedFiles)
        {
            QueuedFiles = queuedFiles;
        }

        public Boolean IsSuccessfullyFinished => Status == ReadingState.Finished;
        
        public void StartReading()
        {
            try
            {
                InternalStartReading();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); // Наверно тут что-то более интеллектуальное должно быть
            }
        }

        private void InternalStartReading()
        {
            _resetEvent.WaitOne();
            Status = ReadingState.Reading;

            foreach (LocalFile2 queuedFile in QueuedFiles)
            {
                if (!queuedFile.IsExist)
                {
                    ReadedBlocks.CompleteAdding();
                    Status = ReadingState.Interrupted;
                    _resetEvent.Set();
                    throw new FileNotFoundException(queuedFile.FileInfo.FilePath);
                }
                
                ReadBlocks(queuedFile);
            }
            Task.WaitAll(_blockPushingTasks.ToArray());
            ReadedBlocks.CompleteAdding();
            Status = ReadingState.Finished;
            _resetEvent.Set();
        }

        private void ReadBlocks(LocalFile2 file)
        {
            Guid fileId = Guid.NewGuid();
            using FileStream fileStream = File.OpenRead(file.FileInfo.FilePath);

            Int64 length = fileStream.Length;
            Int32 bufferSize = length < BlockSize ? (Int32)length : BlockSize;

            Int32 blockCount = 1;

            while (ReadBlock(fileStream, bufferSize, out Buffer readedBlock) > 0)
            {
                FileBlock block = new FileBlock(readedBlock, blockCount, fileId, false);
                blockCount++;

                Task pushing = Task.Run(() => ReadedBlocks.Add(block));
                _blockPushingTasks.Add(pushing);
            }
        }

        private Int32 ReadBlock(Stream stream, Int32 bufferSize, out Buffer buffer)
        {
            buffer = GetBuffer(bufferSize);
            Int32 readedCount = stream.Read(buffer.ByteBuffer, 0, bufferSize);

            if (readedCount != bufferSize)
                buffer.UpdateActualSize(readedCount);

            if (readedCount <= 0)
                buffer.Dispose();

            return readedCount;
        }
        
        private Buffer GetBuffer(Int32 size) => new Buffer(ArrayPool<Byte>.Shared.Rent(size), size, true);

        public void Dispose()
        {
            ReadedBlocks?.Dispose();
        }
    }
}