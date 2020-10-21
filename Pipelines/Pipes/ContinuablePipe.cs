using System;
using System.Collections.Concurrent;

namespace FolderComparer.Pipes
{
    public abstract class ContinuablePipe<TOutput> : Pipe
    {
        public IPipeStartItem<TOutput> PipeItem { get; }

        protected ContinuablePipe(IPipeStartItem<TOutput> item)
        {
            PipeItem = item;
        }

        public Pipe<TOutput, TNext> ContinueWith<TNext>(
            Func<BlockingCollection<TOutput>, BlockingCollection<TNext>, IPipeMiddleItem<TOutput, TNext>> creator)
        {
            return ContinueWith(creator, new ConcurrentQueue<TNext>());
        }

        public Pipe<TOutput, TNext> ContinueWith<TNext>(
          Func<BlockingCollection<TOutput>, BlockingCollection<TNext>, IPipeMiddleItem<TOutput, TNext>> creator,
          IProducerConsumerCollection<TNext> outputConnection)
        {
            return ContinueWith(creator, new BlockingCollection<TNext>(outputConnection));
        }

        public Pipe<TOutput, TNext> ContinueWith<TNext>(
           Func<BlockingCollection<TOutput>, BlockingCollection<TNext>, IPipeMiddleItem<TOutput, TNext>> creator,
           BlockingCollection<TNext> outputConnection)
        {
            var pipeItem = creator.Invoke(PipeItem.Output, outputConnection);

            return ContinueWith(pipeItem);
        }

        public Pipe<TOutput, TNext> ContinueWith<TNext>(
            Action<BlockingCollection<TOutput>, BlockingCollection<TNext>> action)
        {
            return ContinueWith(action, new ConcurrentQueue<TNext>());
        }

        public Pipe<TOutput, TNext> ContinueWith<TNext>(
            Action<BlockingCollection<TOutput>, BlockingCollection<TNext>> action, IProducerConsumerCollection<TNext> outputConnection)
        {
            return ContinueWith(action, new BlockingCollection<TNext>(outputConnection));
        }

        public Pipe<TOutput, TNext> ContinueWith<TNext>(
          Action<BlockingCollection<TOutput>, BlockingCollection<TNext>> action, BlockingCollection<TNext> outputConnection)
        {
            var pipeAction = new PipeAction<TOutput, TNext>(action, PipeItem.Output, outputConnection);

            return ContinueWith(pipeAction);
        }

        internal Pipe<TOutput, TNext> ContinueWith<TNext>(IPipeMiddleItem<TOutput, TNext> item)
        {
            return new Pipe<TOutput, TNext>(item, this);
        }
    }
}
