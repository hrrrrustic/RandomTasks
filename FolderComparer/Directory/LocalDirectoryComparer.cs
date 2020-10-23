using FolderComparer.Files;
using Pipelines.Pipes;
using FolderComparer.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using FolderComparer.Handlers;

namespace FolderComparer
{
    public class LocalDirectoryComparer : IDirectoryComparer<LocalDirectory, ILocalFile, LocalDirectory, ILocalFile>
    {
        public DirectoryCompareResult Compare(DirectoryInfo x, DirectoryInfo y)
        {
            return Compare(new LocalDirectory(x), new LocalDirectory(y));
        }

        public DirectoryCompareResult Compare(LocalDirectory x, LocalDirectory y)
        {
            var pipeline = Pipeline
                .Start<ILocalFile>(output => new FileEnumerator(output, x, y))
                .ContinueWith<IHashableFileBlock>((input, output) => new SingleThreadLocalFileBlocksReader(input, output))
                .ContinueWith<IHashedFileBlock>((input, output) => new FileBlockHasher(SHA512.Create(), input, output))
                .ContinueWith<IHashedFile>((input, output) => new HashedFileBlockMerger(SHA512.Create(), input, output))
                .ContinueWith<IHashedDirectory>((input, output) => new HashedFileMerger(input, output))
                .FinishWith(CompareFolders);

            return pipeline.GetResult();
        }

        private DirectoryCompareResult CompareFolders(BlockingCollection<IHashedDirectory> source)
        {
            foreach (var item in source.GetConsumingEnumerable())
            {
                Console.WriteLine(item.Hash);
            }

            throw new Exception();
        }
    }
}