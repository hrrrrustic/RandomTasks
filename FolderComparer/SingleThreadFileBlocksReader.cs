using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FolderComparer.Blocks;
using FolderComparer.Files;
using FolderComparer.Tools;

namespace FolderComparer
{
    public sealed class SingleThreadFileBlocksReader
    {
        private const Int32 BlockSize = 1024;
        private readonly List<Task> _blockPushingTasks = new();
        private static readonly AutoResetEvent _resetEvent = new(true);
        public readonly BlockingCollection<FileBlock> ReadedBlocks = new(new ConcurrentQueue<FileBlock>());
        public readonly IReadOnlyCollection<LocalFile> QueuedFiles;
        public ReadingState Status { get; private set; } = ReadingState.NotStarted;

        public SingleThreadFileBlocksReader(IReadOnlyCollection<LocalFile> queuedFiles)
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

            foreach (LocalFile queuedFile in QueuedFiles)
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

        private void ReadBlocks(LocalFile file)
        {
            LocalFileInfo info = file.FileInfo;
            using FileStream fileStream = File.OpenRead(info.FilePath);

            Int64 length = fileStream.Length;
            Int32 bufferSize = length < BlockSize ? (Int32)length : BlockSize;

            Int32 blockCount = 1;

            while (ReadBlock(fileStream, bufferSize, out Byte[] readedBlock) > 0)
            {
                FileBlock block = new FileBlock(readedBlock, blockCount, info);
                blockCount++;

                Task pushing = Task.Run(() => ReadedBlocks.Add(block));
                _blockPushingTasks.Add(pushing);
            }
        }

        private Int32 ReadBlock(Stream stream, Int32 bufferSize, out Byte[] readedBlock)
        {
            readedBlock = Array.Empty<Byte>(); 

            Byte[] buffer = GetBuffer(bufferSize);
            Int32 readedCount = stream.Read(buffer, 0, bufferSize);

            if (readedCount > 0)
                readedBlock = buffer;

            return readedCount;
        }
        
        private Byte[] GetBuffer(Int32 size)
        {
            if (size < 1024)
                return new Byte[size];

            return ArrayPool<Byte>.Shared.Rent(size);
        }   
    }
}