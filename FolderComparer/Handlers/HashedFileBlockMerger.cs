using Pipelines.Pipes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer.Handlers
{
    public class HashedFileBlockMerger : IPipeMiddleItem<IHashedFileBlock, IHashedFlie>
    {
        public BlockingCollection<IHashedFileBlock> Input { get; }
        public BlockingCollection<IHashedFlie> Output { get; }
        private readonly Dictionary<Guid, List<IHashedFileBlock>> _fileBlocks = new();

        public HashedFileBlockMerger(BlockingCollection<IHashedFileBlock> source, BlockingCollection<IHashedFlie> destination)
        {
            Input = source;
            Output = destination;
        }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
        }

        public void StartHandling()
        {
            foreach (var hashedBlock in Input.GetConsumingEnumerable())
            {
                if (!_fileBlocks.ContainsKey(hashedBlock.FileId))
                    _fileBlocks.Add(hashedBlock.FileId, new List<IHashedFileBlock>());

                _fileBlocks[hashedBlock.FileId].Add(hashedBlock);
                if (hashedBlock.IsLastBlock)
                {
                    Output.Add(MergeBlocks(hashedBlock.FileId));
                    _fileBlocks.Remove(hashedBlock.FileId);
                }
            }
        }
        public void Execute()
        {
            StartHandling();
            Output.CompleteAdding();
        }

        private IHashedFlie MergeBlocks(Guid key)
        {
            var orderedBlocks = _fileBlocks[key].OrderBy(k => k.BlockNumber).ToList();
            IHashedFileBlock resultHashedBlock = orderedBlocks[0];

            for (int i = 1; i < orderedBlocks.Count; i++)
                resultHashedBlock = resultHashedBlock.MergeHash(orderedBlocks[i]);

            throw new Exception();
        }
    }
}
