using FolderComparer.Pipes;
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
    public class SingleThreadLocalFileBlocksReader2 //: IPipeMiddleItem<ILocalFile, IFileBlock>
    {
        private readonly IProducerConsumerCollection<ILocalFile> _queuedFiles;
        private static readonly AutoResetEvent _resetEvent = new(true);

        public IProducerConsumerCollection<IFileBlock> Output { get; init; }

        public IProducerConsumerCollection<ILocalFile> Input => _queuedFiles;

        public SingleThreadLocalFileBlocksReader2(IProducerConsumerCollection<IFileBlock> destination)
        {
            Output = destination;
        }


        public static void StartReading(IProducerConsumerCollection<IFileBlock> dest)
        {
            throw new NotImplementedException();
        }

        public ILocalFile GetItem()
        {
            throw new NotImplementedException();
        }
    }
}
