using System;
using System.Collections.Concurrent;

namespace FolderComparer.Pipes
{
    public abstract class Pipe
    {
        internal abstract void Execute();
    }

    public class Pipe<TIn, TOut> : ContinuablePipe<TOut>
    {
        private readonly ContinuablePipe<TIn> _prevPipe;

        internal Pipe(IPipeMiddleItem<TIn, TOut> pipeAction, ContinuablePipe<TIn> prevPipe) : base(pipeAction)
        {
            _prevPipe = prevPipe;
        }

        public FinishPipe<TOut, TResult> FinishWith<TResult>(Func<BlockingCollection<TOut>, TResult> func)
        {
            return new FinishPipe<TOut, TResult>(func, PipeItem.Output, this);
        }

        internal override void Execute()
        {
            _prevPipe.Execute();
            PipeItem.Execute();
        }
    }
}
