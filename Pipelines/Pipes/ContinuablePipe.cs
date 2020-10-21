using System;
using System.Collections.Concurrent;

namespace Pipelines.Pipes
{
    public abstract partial class ContinuablePipe<TOut> : Pipe
    {
        public IPipeStartItem<TOut> PipeItem { get; }

        protected ContinuablePipe(IPipeStartItem<TOut> item, bool isParallel) : base(isParallel)
        {
            PipeItem = item;
        }

        public Pipe<TOut, TNextOut> ContinueWith<TNextOut>(IPipeMiddleItem<TOut, TNextOut> item)
        {
            return ContinueWith(item, false);
        }

        public Pipe<TOut, TNextOut> ContinueParallelWith<TNextOut>(IPipeMiddleItem<TOut, TNextOut> item)
        {
            return ContinueWith(item, true);
        }

        private Pipe<TOut, TNextOut> ContinueWith<TNextOut>(IPipeMiddleItem<TOut, TNextOut> item, bool IsParallel)
        {
            return new Pipe<TOut, TNextOut>(item, this, IsParallel);
        }
    }
}
