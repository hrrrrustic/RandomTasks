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
                    .WithCancellation(default)
                    .ContinueWith<IFileBlock>((input, output) => new SingleThreadLocalFileBlocksReader(input, output))
                    .ContinueWith<IHashedFileBlock>((input, output) => new FileBlockHandler(input, output))
                    .ContinueWith<IHashedFlie>((input, output) => new HashedFileBlockHandler(input, output))
                    .ContinueWith<IHashedDirectory>((input, output) => new HashedFileHandler(input, output))
                    .FinishWith(CompareFolders);

            return pipeline.GetResult();
        }

        private DirectoryCompareResult CompareFolders(BlockingCollection<IHashedDirectory> source)
        {
            throw new Exception();
        }
    }
}