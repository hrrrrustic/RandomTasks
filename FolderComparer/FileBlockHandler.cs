﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Pipelines.Pipes;

namespace FolderComparer
{
    public class FileBlockHandler : IPipeMiddleItem<IFileBlock, IHashedFileBlock>
    {
        public BlockingCollection<IFileBlock> Input { get; }
        public BlockingCollection<IHashedFileBlock> Output { get; }

        public FileBlockHandler(BlockingCollection<IFileBlock> source, BlockingCollection<IHashedFileBlock> destination)
        {
            Input = source;
            Output = destination;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
        }
    }
    public class HashedFileBlockHandler : IPipeMiddleItem<IHashedFileBlock, IHashedFlie>
    {
        public HashedFileBlockHandler(BlockingCollection<IHashedFileBlock> source, BlockingCollection<IHashedFlie> destination)
        {
            Input = source;
            Output = destination;
        }

        public BlockingCollection<IHashedFileBlock> Input { get; }

        public BlockingCollection<IHashedFlie> Output { get; }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
    public class HashedFileHandler : IPipeMiddleItem<IHashedFlie, IHashedDirectory>
    {
        public BlockingCollection<IHashedFlie> Input { get; }
        public BlockingCollection<IHashedDirectory> Output { get; }

        public HashedFileHandler(BlockingCollection<IHashedFlie> source, BlockingCollection<IHashedDirectory> destination)
        {
            Input = source;
            Output = destination;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
        }
    }
}