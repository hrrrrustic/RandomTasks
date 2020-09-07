using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FolderComparer.Blocks;

namespace FolderComparer
{
    public class FileBlockPool 
        //TODO : пофиксить диспоуз
        // TODO : ренейм
    {
        public readonly BlockingCollection<FileBlock> Blocks = SingleThreadFileBlocksReader.GetInstance().ReadedBlocks;
        public readonly BlockingCollection<HashedFileBlock> HashedBlocks = new BlockingCollection<HashedFileBlock>(new ConcurrentQueue<HashedFileBlock>());
        public void HandleBlocks()
        {
            List<Task> hashTasks = new List<Task>();
            while (!Blocks.IsCompleted)
            {
                FileBlock currentBlock;
                try
                {
                    currentBlock = Blocks.Take();
                }
                catch (InvalidOperationException)
                {
                    continue;
                }

                hashTasks.Add(Task.Run(() => HashBlock(currentBlock)));
            }

            Task.WaitAll(hashTasks.ToArray());
        }

        private void HashBlock(FileBlock block)
        {
            HashedFileBlock hashedBlock = block.HashBlock(SHA512.Create());
            HashedBlocks.Add(hashedBlock);
        }
    }
}