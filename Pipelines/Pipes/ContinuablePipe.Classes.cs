using Pipelines.Pipes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pipelines.Pipes
{
    public abstract partial class ContinuablePipe<TOut>
    {
        public Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
            Func<BlockingCollection<TOut>, BlockingCollection<TNextOut>, IPipeMiddleItem<TOut, TNextOut>> creator)
        {
            return ContinueWith(creator, false);
        }

        public Pipe<TOut, TNextOut> ContinueParallelWith<TNextOut>(
            Func<BlockingCollection<TOut>, BlockingCollection<TNextOut>, IPipeMiddleItem<TOut, TNextOut>> creator)
        {
            return ContinueWith(creator, true);
        }

        private Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
            Func<BlockingCollection<TOut>, BlockingCollection<TNextOut>, IPipeMiddleItem<TOut, TNextOut>> creator, bool IsParallel)
        {
            return ContinueWith(creator, new ConcurrentQueue<TNextOut>(), IsParallel);
        }
        public Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
          Func<BlockingCollection<TOut>, BlockingCollection<TNextOut>, IPipeMiddleItem<TOut, TNextOut>> creator,
          IProducerConsumerCollection<TNextOut> outputConnection)
        {
            return ContinueWith(creator, outputConnection, false);
        }

        public Pipe<TOut, TNextOut> ContinueParallelWith<TNextOut>(
          Func<BlockingCollection<TOut>, BlockingCollection<TNextOut>, IPipeMiddleItem<TOut, TNextOut>> creator,
          IProducerConsumerCollection<TNextOut> outputConnection)
        {
            return ContinueWith(creator, outputConnection, true);
        }

        private Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
          Func<BlockingCollection<TOut>, BlockingCollection<TNextOut>, IPipeMiddleItem<TOut, TNextOut>> creator,
          IProducerConsumerCollection<TNextOut> outputConnection, bool isParallel)
        {
            return ContinueWith(creator, new BlockingCollection<TNextOut>(outputConnection), isParallel);
        }

        public Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
           Func<BlockingCollection<TOut>, BlockingCollection<TNextOut>, IPipeMiddleItem<TOut, TNextOut>> creator,
           BlockingCollection<TNextOut> outputConnection)
        {
            return ContinueWith(creator, outputConnection, false);
        }

        public Pipe<TOut, TNextOut> ContinueParallelWith<TNextOut>(
           Func<BlockingCollection<TOut>, BlockingCollection<TNextOut>, IPipeMiddleItem<TOut, TNextOut>> creator,
           BlockingCollection<TNextOut> outputConnection)
        {
            return ContinueWith(creator, outputConnection, true);
        }

        private Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
          Func<BlockingCollection<TOut>, BlockingCollection<TNextOut>, IPipeMiddleItem<TOut, TNextOut>> creator,
          BlockingCollection<TNextOut> outputConnection, bool isParallel)
        {
            var pipeItem = creator.Invoke(PipeItem.Output, outputConnection);

            return ContinueWith(pipeItem, isParallel);
        }
    }
}
