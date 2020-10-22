using Pipelines.Pipes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer.Handlers
{
    public class FileBlockHasher : IPipeMiddleItem<IFileBlock, IHashedFileBlock>
    {
        public BlockingCollection<IFileBlock> Input { get; }
        public BlockingCollection<IHashedFileBlock> Output { get; }
        private readonly HashAlgorithm _algorithm;

        public FileBlockHasher(HashAlgorithm algorithm, BlockingCollection<IFileBlock> source, BlockingCollection<IHashedFileBlock> destination)
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
