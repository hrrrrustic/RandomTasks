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
        private readonly WaitConcurrentQueue<ILocalFile> _queuedFiles = new();
        private static readonly AutoResetEvent _resetEvent = new(true);

        public IAddingCompletableCollection<IFileBlock> Output { get; init; }

        public IAddingCompletableCollection<ILocalFile> Input => _queuedFiles;

        public SingleThreadLocalFileBlocksReader2(IAddingCompletableCollection<IFileBlock> destination)
        {
            Output = destination;
        }

        public void PushItem(ILocalFile item)
        {
            _queuedFiles.PushItem(item);
        }

        public static void StartReading(IAddingCompletableCollection<IFileBlock> dest)
        {
            throw new NotImplementedException();
        }

        public void StartReading()
        {
            _resetEvent.WaitOne();

            while (!_queuedFiles.IsEmpty)
            {
                if (_queuedFiles.AddingCompleted)
                    return;

                _queuedFiles.GetItem();
            }

            _resetEvent.Set();

            throw new NotImplementedException();
        }

        public void CompletePushing()
        {
            _queuedFiles.CompletePushing();
        }

        public ILocalFile GetItem()
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            StartReading();
        }
    }
}
