using Pipelines.Pipes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer.Handlers
{
    public class HashedFileMerger : IPipeMiddleItem<IHashedFlie, IHashedDirectory>
    {
        public BlockingCollection<IHashedFlie> Input { get; }
        public BlockingCollection<IHashedDirectory> Output { get; }
        private readonly Dictionary<Guid, List<IHashedFlie>> _files = new();

        public HashedFileMerger(BlockingCollection<IHashedFlie> source, BlockingCollection<IHashedDirectory> destination)
        {
            Input = source;
            Output = destination;
        }

        public void StartHandling()
        {
            foreach (var hashedFile in Input.GetConsumingEnumerable())
            {
                if (!_files.ContainsKey(hashedFile.FolderId))
                    _files.Add(hashedFile.FolderId, new List<IHashedFlie>());

                _files[hashedFile.FolderId].Add(hashedFile);
            }


            foreach (var folder in _files.Keys)
            {
                Output.Add(MergeHashedFiles(folder));
            }
        }

        private IHashedDirectory MergeHashedFiles(Guid key)
        {
            throw new Exception();
        }
        public void Execute()
        {
            StartHandling();
            Output.CompleteAdding();
        }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
        }
    }
}
