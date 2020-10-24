using FolderComparer.Files;
using Pipelines.Pipes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace FolderComparer.Handlers
{
    public sealed class HashedFileBlockMerger : IPipeMiddleItem<IHashedFileBlock, IHashedFile>
    {
        public BlockingCollection<IHashedFileBlock> Input { get; }
        public BlockingCollection<IHashedFile> Output { get; }
        private readonly Dictionary<Guid, List<IHashedFileBlock>> _fileBlocks = new();
        private readonly HashAlgorithm _hashAlgorithm;

        public HashedFileBlockMerger(HashAlgorithm hashAlgorithm,
            BlockingCollection<IHashedFileBlock> source, BlockingCollection<IHashedFile> destination)
        {
            Input = source;
            Output = destination;
            _hashAlgorithm = hashAlgorithm;
        }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
            _hashAlgorithm.Dispose();
        }

        public void StartHandling()
        {
            foreach (var hashedBlock in Input.GetConsumingEnumerable())
            {
                if (!_fileBlocks.ContainsKey(hashedBlock.FileInfo.FileId))
                    _fileBlocks.Add(hashedBlock.FileInfo.FileId, new List<IHashedFileBlock>());

                _fileBlocks[hashedBlock.FileInfo.FileId].Add(hashedBlock);
                if (hashedBlock.IsLastBlock)
                    Output.Add(MergeBlocks(hashedBlock.FileInfo.FileId));
            }
        }
        public void Execute()
        {
            StartHandling();
            Output.CompleteAdding();
        }

        private IHashedFile MergeBlocks(Guid key)
        {
            var orderedBlocks = _fileBlocks[key].OrderBy(k => k.BlockNumber).ToList();
            byte[] resultHash = new byte[_hashAlgorithm.HashSize / 8];

            for (int i = 0; i < orderedBlocks.Count; i++)
                resultHash = orderedBlocks[i].MergeHash(resultHash);

            return new HashedLocalFile(resultHash, orderedBlocks[0].FileInfo);
        }
    }
}
