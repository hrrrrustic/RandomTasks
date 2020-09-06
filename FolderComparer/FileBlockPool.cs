using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace FolderComparer
{
    public class FileBlockPool //TODO : пофиксить диспоуз
    {
        public BlockingCollection<FileBlock> Blocks = SingleThreadFileBlocksReader.GetInstance().ReadedBlocks;
       /// public ConcurrentDictionary<Guid, Dictionary<Guid, List<FileBlock>>> // TODO : Interlocked.Increment, file count in folder
        public void HandleBlocks()
        {
            while (!Blocks.IsCompleted)
            {
                FileBlock currentBlock = Blocks.Take();
                
            }
        }
    }
}