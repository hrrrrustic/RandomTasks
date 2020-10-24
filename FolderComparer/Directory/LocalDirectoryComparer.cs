using Pipelines.Pipes;
using FolderComparer.Tools;
using System;
using System.IO;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using FolderComparer.Handlers;

namespace FolderComparer
{
    public class LocalDirectoryComparer : IDirectoryComparer<LocalDirectory, LocalDirectory>
    {
        public DirectoryCompareResult Compare(DirectoryInfo x, DirectoryInfo y)
        {
            return Compare(new LocalDirectory(x), new LocalDirectory(y));
        }

        public DirectoryCompareResult Compare(LocalDirectory x, LocalDirectory y)
        {
            var pipeline = Pipeline
                .Start<IFile>(output => new FileEnumerator(output, x, y))
                .ContinueWith<IHashableFileBlock>((input, output) => new SingleThreadLocalFileBlocksReader(input, output))
                .ContinueWith<IHashedFileBlock>((input, output) => new FileBlockHasher(SHA512.Create(), input, output))
                .ContinueWith<IHashedFile>((input, output) => new HashedFileBlockMerger(SHA512.Create(), input, output))
                .ContinueWith<IHashedDirectory>((input, output) => new HashedFileMerger(input, output))
                .FinishWith(CompareFolders);

            return pipeline.GetResult();
        }

        private DirectoryCompareResult CompareFolders(BlockingCollection<IHashedDirectory> source)
        {
            var folders = source.ToArray();

            if (folders.Length != 2)
                throw new NotSupportedException("Comparing more than 2 folders not supported");

            var firstFolder = folders[0];
            var secondFolder = folders[1];

            return firstFolder.Compare(secondFolder);
        }
    }
}