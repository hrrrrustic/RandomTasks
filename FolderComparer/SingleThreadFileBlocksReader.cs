using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using FolderComparer.Blocks;
using FolderComparer.Files;
using FolderComparer.Tools;
using FileInfo = FolderComparer.Files.FileInfo;
using Int32 = System.Int32;

namespace FolderComparer
{
    public sealed class SingleThreadFileBlocksReader : IDisposable
    {
        private const Int32 True = 1;
        private const Int32 False = 0;
        private Int32 _readingFlag = False;

        private static readonly Lazy<SingleThreadFileBlocksReader> Instance = new Lazy<SingleThreadFileBlocksReader>(() => new SingleThreadFileBlocksReader());
        public static SingleThreadFileBlocksReader GetInstance() => Instance.Value;
        public readonly BlockingCollection<LocalFile> QueuedFiles = new BlockingCollection<LocalFile>(new ConcurrentQueue<LocalFile>());
        public readonly BlockingCollection<FileBlock> ReadedBlocks = new BlockingCollection<FileBlock>(new ConcurrentQueue<FileBlock>());

        private readonly List<Task> _blockPushingTasks = new List<Task>();

        private SingleThreadFileBlocksReader() { }
        public void StartReading()
        {
            if (Interlocked.CompareExchange(ref _readingFlag, True, False) == True)
                throw new MultithreadAccessException("Multithread reading is not allowed");

            while (!QueuedFiles.IsCompleted)
            {
                LocalFile file;
                try
                {
                    file = QueuedFiles.Take();
                }
                catch (InvalidOperationException)
                {
                    break;
                }

                String filePath = file.FileInfo.FilePath;
                if (!File.Exists(filePath))
                    throw new FileNotFoundException(filePath);

                ReadBlocks(file);
            }

            _readingFlag = False;

            Task.WaitAll(_blockPushingTasks.ToArray());
            ReadedBlocks.CompleteAdding(); // TODO: из-за этого в целом флаг _readingFlag становится около бесполезным. Даже если чтение закончилось, после этого не получится начать чтение заново. Нужно сделать флаг статическим, сделать класс не синглтоном и сделать синхронизацию на ResetEvent'е
        }

        private void ReadBlocks(LocalFile file)
        {
            const Int32 blockSize = 1024;

            FileInfo info = file.FileInfo;
            FileStream fileStream = File.OpenRead(info.FilePath);

            Int64 length = fileStream.Length;
            Int32 bufferSize = length < blockSize ? (Int32)length : blockSize;

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
            readedBlock = null;

            Byte[] buffer = ArrayPool<Byte>.Shared.Rent(bufferSize); // TODO : на маленьких файлах лучше просто аллоцировать
            Int32 readedCount = stream.Read(buffer, 0, bufferSize);

            if (readedCount > 0)
                readedBlock = buffer;

            return readedCount;
        }

        public void Dispose()
        {
            QueuedFiles?.Dispose();
            ReadedBlocks?.Dispose();
        }

        ~SingleThreadFileBlocksReader() => Dispose();
    }
}