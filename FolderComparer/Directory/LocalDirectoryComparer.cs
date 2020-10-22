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
                .ContinueParallelWith<IHashableFileBlock>((input, output) => new SingleThreadLocalFileBlocksReader(input, output))
                .ContinueParallelWith<IHashedFileBlock>((input, output) => new FileBlockHasher(SHA512.Create(), input, output))
                .ContinueParallelWith<IHashedFlie>((input, output) => new HashedFileBlockMerger(input, output))
                .ContinueParallelWith<IHashedDirectory>((input, output) => new HashedFileMerger(input, output))
                .FinishParallelWith(Test);

            pipeline.GetResult();
            return null;
        }
        private int Test(BlockingCollection<IHashedDirectory> input)
        {
            foreach (var item in input.GetConsumingEnumerable())
            {
                Console.WriteLine(item.Hash);
            }

            return 1;
        }
        private DirectoryCompareResult CompareFolders(BlockingCollection<IHashedDirectory> source)
        {
            throw new Exception();
        }
    }
}