using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Pipelines.Pipes
{
    public abstract class Pipe
    {
        protected bool _isParallel { get; }
        protected Pipe(bool isParallel)
        {
            _isParallel = isParallel;
        }
        internal abstract void Execute();
    }

    public class Pipe<TIn, TOut> : ContinuablePipe<TOut>
    {
        private readonly ContinuablePipe<TIn> _prevPipe;

        internal Pipe(IPipeMiddleItem<TIn, TOut> pipeAction, ContinuablePipe<TIn> prevPipe, bool isParallel) : base(pipeAction, isParallel)
        {
            _prevPipe = prevPipe;
        }

        public FinishPipe<TOut, TResult> FinishWith<TResult>(Func<BlockingCollection<TOut>, TResult> func)
        {
            return new FinishPipe<TOut, TResult>(func, PipeItem.Output, this, false);
        }

        internal override void Execute()
        {
            _prevPipe.Execute();
            PipeItem.Execute();
        }
    }
}
