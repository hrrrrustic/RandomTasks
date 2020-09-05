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

        public readonly BlockingCollection<String> FilePaths = new BlockingCollection<String>(new ConcurrentQueue<String>());
        public readonly BlockingCollection<FileBlock> ReadedBlocks;

        public SingleThreadFileBlocksReader(BlockingCollection<FileBlock> blockDestination)
        {
            ReadedBlocks = blockDestination;
        }

        public void StartReading()
        {
            if (Interlocked.CompareExchange(ref _readingFlag, True, False) == True)
                throw new MultithreadAccessException("Multithread reading is not allowed");

            foreach (String filePath in FilePaths) //TODO : while with resetEvent
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException(filePath);

                FileStream fileStream = File.OpenRead(filePath);
                ReadBlocks(fileStream);
            }

            _readingFlag = False;
        }

        private void ReadBlocks(FileStream stream)
        {
            Int64 length = stream.Length; 
            Int32 bufferSize = length < 4096 ? (Int32)length : 4096;

            while (ReadBlock(stream, bufferSize, out Byte[] readedBlock) > 0)
            {
                FileBlock block = new FileBlock(readedBlock);
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