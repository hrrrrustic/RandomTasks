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
            Action<BlockingCollection<TOut>, BlockingCollection<TNextOut>> action)
        {
            return ContinueWith(action, false);
        }

        public Pipe<TOut, TNextOut> ContinueParallelWith<TNextOut>(
            Action<BlockingCollection<TOut>, BlockingCollection<TNextOut>> action)
        {
            return ContinueWith(action, true);
        }

        private Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
            Action<BlockingCollection<TOut>, BlockingCollection<TNextOut>> action, bool isParallel)
        {
            return ContinueWith(action, new ConcurrentQueue<TNextOut>(), isParallel);
        }

        public Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
            Action<BlockingCollection<TOut>, BlockingCollection<TNextOut>> action, IProducerConsumerCollection<TNextOut> outputConnection)
        {
            return ContinueWith(action, outputConnection, false);
        }

        public Pipe<TOut, TNextOut> ContinueParallelWith<TNextOut>(
            Action<BlockingCollection<TOut>, BlockingCollection<TNextOut>> action, IProducerConsumerCollection<TNextOut> outputConnection)
        {
            return ContinueWith(action, outputConnection, true);
        }

        private Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
            Action<BlockingCollection<TOut>, BlockingCollection<TNextOut>> action,
            IProducerConsumerCollection<TNextOut> outputConnection,
            bool isParallel)
        {
            return ContinueWith(action, new BlockingCollection<TNextOut>(outputConnection), isParallel);
        }

        public Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
          Action<BlockingCollection<TOut>, BlockingCollection<TNextOut>> action, BlockingCollection<TNextOut> outputConnection)
        {
            return ContinueWith(action, outputConnection, false);
        }
        public Pipe<TOut, TNextOut> ContinueParallelWith<TNextOut>(
          Action<BlockingCollection<TOut>, BlockingCollection<TNextOut>> action, BlockingCollection<TNextOut> outputConnection)
        {
            return ContinueWith(action, outputConnection, true);
        }
        private Pipe<TOut, TNextOut> ContinueWith<TNextOut>(
          Action<BlockingCollection<TOut>, BlockingCollection<TNextOut>> action, BlockingCollection<TNextOut> outputConnection, bool isParallel)
        {
            var pipeAction = new PipeAction<TOut, TNextOut>(action, PipeItem.Output, outputConnection);

            return ContinueWith(pipeAction, isParallel);
        }
    }
}
