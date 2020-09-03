using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

namespace FolderComparer
{
    public class SingleThreadFileBlocksReader
    {
        private const Int32 True = 1;
        private const Int32 False = 0;
 
        private readonly String[] _filePaths;
        public ConcurrentQueue<LocalFile> CompletedFiles = new ConcurrentQueue<LocalFile>();

        private Int32 _readingFlag = 0;

        public SingleThreadFileBlocksReader(IReadOnlyCollection<String> filePaths, IProducerConsumerCollection<FileBlock> blockDestination) : this(filePaths.ToArray(), blockDestination)
        { }

        public SingleThreadFileBlocksReader(String[] filePaths, IProducerConsumerCollection<FileBlock> blockDestination)
        {
            _filePaths = filePaths;
        }

        public void StartReading()
        {
            if (Interlocked.CompareExchange(ref _readingFlag, True, False) == True)
                throw new Exception("Multithread reading is not allowed"); // TODO : нормальный эксепшн

            foreach (String filePath in _filePaths)
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException(filePath);

                LocalFile localFile = new LocalFile(filePath);
                localFile.ComputeHashCode(SHA512.Create());

                // TODO : Переделать на чтение блоков и помещение их в очередь
            }

        }
    }
}