using FolderComparer.Directory;
using Pipelines.Pipes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FolderComparer.Handlers
{
    public sealed class HashedFileMerger : IPipeMiddleItem<IHashedFile, IHashedDirectory>
    {
        public BlockingCollection<IHashedFile> Input { get; }
        public BlockingCollection<IHashedDirectory> Output { get; }
        private readonly Dictionary<Guid, List<IHashedFile>> _files = new();
        public HashedFileMerger(BlockingCollection<IHashedFile> source, BlockingCollection<IHashedDirectory> destination)
        {
            Input = source;
            Output = destination;
        }

        public void StartHandling()
        {
            foreach (var hashedFile in Input.GetConsumingEnumerable())
            {
                if (!_files.ContainsKey(hashedFile.DirectoryId))
                    _files.Add(hashedFile.DirectoryId, new List<IHashedFile>());

                _files[hashedFile.DirectoryId].Add(hashedFile);
            }

            foreach (var folder in _files.Keys)
            {
                Output.Add(MergeHashedFiles(folder));
            }
        }

        private IHashedDirectory MergeHashedFiles(Guid key)
        {
            var files = _files[key];
            byte[] resultHash = new byte[files[0].Hash.Length];

            for (int i = 0; i < files.Count; i++)
                resultHash = files[i].MergeHash(resultHash);

            return new HashedLocalDirectory(resultHash, _files[key]);
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