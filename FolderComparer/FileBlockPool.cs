using System.Collections.Concurrent;

namespace FolderComparer
{
    public class FileBlockPool //TODO : пофиксить диспоуз
    {
        public BlockingCollection<FileBlock> Blocks = new BlockingCollection<FileBlock>(new ConcurrentQueue<FileBlock>());

        public void HandleBlocks()
        {
            while (!Blocks.IsCompleted)
            {
                FileBlock currentBlock = Blocks.Take();
            }
        }
    }
}