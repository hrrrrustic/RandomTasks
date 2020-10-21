using System;
using System.Collections.Concurrent;

namespace FolderComparer.Pipes
{
    public abstract class ContinuablePipe<TOut> : Pipe
    {
        public IPipeStartItem<TOut> PipeItem { get; }

        protected ContinuablePipe(IPipeStartItem<TOut> item)
        {
            PipeItem = item;
        }

        public Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
            Func<BlockingCollection<TOut>, BlockingCollection<TNextOut>, IPipeMiddleItem<TOut, TNextOut>> creator)
        {
            return ContinueWith(creator, new ConcurrentQueue<TNextOut>());
        }

        public Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
          Func<BlockingCollection<TOut>, BlockingCollection<TNextOut>, IPipeMiddleItem<TOut, TNextOut>> creator,
          IProducerConsumerCollection<TNextOut> outputConnection)
        {
            return ContinueWith(creator, new BlockingCollection<TNextOut>(outputConnection));
        }

        public Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
           Func<BlockingCollection<TOut>, BlockingCollection<TNextOut>, IPipeMiddleItem<TOut, TNextOut>> creator,
           BlockingCollection<TNextOut> outputConnection)
        {
            var pipeItem = creator.Invoke(PipeItem.Output, outputConnection);

            return ContinueWith(pipeItem);
        }

        public Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
            Action<BlockingCollection<TOut>, BlockingCollection<TNextOut>> action)
        {
            return ContinueWith(action, new ConcurrentQueue<TNextOut>());
        }

        public Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
            Action<BlockingCollection<TOut>, BlockingCollection<TNextOut>> action, IProducerConsumerCollection<TNextOut> outputConnection)
        {
            return ContinueWith(action, new BlockingCollection<TNextOut>(outputConnection));
        }

        public Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
          Action<BlockingCollection<TOut>, BlockingCollection<TNextOut>> action, BlockingCollection<TNextOut> outputConnection)
        {
            var pipeAction = new PipeAction<TOut, TNextOut>(action, PipeItem.Output, outputConnection);

            return ContinueWith(pipeAction);
        }

        internal Pipe<TOut, TNextOut> ContinueWith<TNextOut>(IPipeMiddleItem<TOut, TNextOut> item)
        {
            return new Pipe<TOut, TNextOut>(item, this);
        }
    }
}
