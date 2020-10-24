using Pipelines.Pipes;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace FolderComparer.Handlers
{
    public sealed class FileBlockHasher : IPipeMiddleItem<IHashableFileBlock, IHashedFileBlock>
    {
        public BlockingCollection<IHashableFileBlock> Input { get; }
        public BlockingCollection<IHashedFileBlock> Output { get; }
        private readonly HashAlgorithm _algorithm;

        public FileBlockHasher(HashAlgorithm algorithm, BlockingCollection<IHashableFileBlock> source, BlockingCollection<IHashedFileBlock> destination)
        {
            Input = source;
            Output = destination;
            _algorithm = algorithm;
        }

        private void Handle()
        {
            foreach (var fileBlock in Input.GetConsumingEnumerable())
            {
                var hashedBlock = fileBlock.HashBlock(_algorithm);
                Output.Add(hashedBlock);
            }
        }

        public void Execute()
        {
            Handle();
            Output.CompleteAdding();
        }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
            _algorithm.Dispose();
        }
    }
}
