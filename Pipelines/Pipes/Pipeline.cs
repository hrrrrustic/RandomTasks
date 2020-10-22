using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Pipelines.Pipes
{
    public static class Pipeline
    {
        public static StartPipe<TOut> StartWithCancellation<TOut>(IPipeStartItem<TOut> pipeItem, CancellationToken cancellation)
        {
            return new StartPipe<TOut>(pipeItem);
        }

        public static StartPipe<TOut> Start<TOut>(IPipeStartItem<TOut> pipeItem)
        {
            return new StartPipe<TOut>(pipeItem);
        }

        public static StartPipe<TOut> Start<TOut>(Func<BlockingCollection<TOut>, IPipeStartItem<TOut>> creator)
        {
            return Start(creator, new ConcurrentQueue<TOut>());
        }

        public static StartPipe<TOut> Start<TOut>(Func<BlockingCollection<TOut>, IPipeStartItem<TOut>> creator,
            IProducerConsumerCollection<TOut> connecntin)
        {
            var pipeAction = creator.Invoke(new BlockingCollection<TOut>(connecntin));

            return Start(pipeAction);
        }
    }
}