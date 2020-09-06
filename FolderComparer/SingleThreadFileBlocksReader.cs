using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class SingleThreadFileBlocksReader
    {
        private const Int32 True = 1;
        private const Int32 False = 0;
        private Int32 _readingFlag = 0;
        public Boolean IsReading => _readingFlag == True;

        private static readonly Lazy<SingleThreadFileBlocksReader> Instance = new Lazy<SingleThreadFileBlocksReader>(() => new SingleThreadFileBlocksReader());
        public static SingleThreadFileBlocksReader GetInstance() => Instance.Value;
        public readonly BlockingCollection<LocalFile> QueuedFiles = new BlockingCollection<LocalFile>(new ConcurrentQueue<LocalFile>());
        public readonly BlockingCollection<FileBlock> ReadedBlocks = new BlockingCollection<FileBlock>(new ConcurrentQueue<FileBlock>());

        private SingleThreadFileBlocksReader() { }
        public void StartReading()
        {
            if (Interlocked.CompareExchange(ref _readingFlag, True, False) == True)
                throw new MultithreadAccessException("Multithread reading is not allowed");

            foreach (LocalFile file in QueuedFiles) //TODO : while with resetEvent
            {
                if (!File.Exists(file.FilePath))
                    throw new FileNotFoundException(file.FilePath);

                ReadBlocks(file);
            }
            _readingFlag = False;
        }

        private void ReadBlocks(LocalFile file)
        {
            FileStream fileStream = File.OpenRead(file.FilePath);

            Int64 length = fileStream.Length; 
            Int32 bufferSize = length < 4096 ? (Int32)length : 4096;

            while (ReadBlock(fileStream, bufferSize, out Byte[] readedBlock) > 0)
            {
                FileBlock block = new FileBlock(readedBlock, file.FolderId, file.FolderId);
                Task.Run(() => ReadedBlocks.Add(block));
            }

        }

        private Int32 ReadBlock(Stream stream, Int32 bufferSize, out Byte[] readedBlock)
        {
            readedBlock = null;

            Byte[] buffer = ArrayPool<Byte>.Shared.Rent(bufferSize); // TODO : на маленьких файлах лучше просто аллоцировать
            Int32 readedCount = stream.Read(buffer, 0, bufferSize);

            if(readedCount > 0)
                readedBlock = buffer;

            return readedCount;
        }
    }
}