using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Pipelines.Pipes
{
    public abstract class Pipe
    {
        protected bool IsParallel { get; }
        protected Pipe(bool isParallel)
        {
            IsParallel = isParallel;
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
            return FinishWith(func, false);
        }

        public FinishPipe<TOut, TResult> FinishParallelWith<TResult>(Func<BlockingCollection<TOut>, TResult> func)
        {
            return FinishWith(func, true);
        }

        private FinishPipe<TOut, TResult> FinishWith<TResult>(Func<BlockingCollection<TOut>, TResult> func, bool isParallel)
        {
            return new FinishPipe<TOut, TResult>(func, PipeItem.Output, this, isParallel);
        }

        internal override void Execute()
        {
            if(IsParallel)
            {
                Thread runThread = new Thread(_prevPipe.Execute);
                runThread.Start();
                PipeItem.Execute();
                runThread.Join();
                return;
            }

            _prevPipe.Execute();
            PipeItem.Execute();
        }
    }
}
