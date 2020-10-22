using Pipelines.Pipes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class SingleThreadLocalFileBlocksReader : IPipeMiddleItem<ILocalFile, IFileBlock>
    {
        private static readonly AutoResetEvent _resetEvent = new(true);

        public BlockingCollection<IFileBlock> Output { get; }

        public BlockingCollection<ILocalFile> Input { get; }

        public SingleThreadLocalFileBlocksReader(BlockingCollection<ILocalFile> source, BlockingCollection<IFileBlock> destination)
        {
            Input = source;
            Output = destination;
        }


        public static void StartReading()
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            StartReading();
        }

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
        }
    }
}
