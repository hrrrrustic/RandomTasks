using System;
using System.Collections.Concurrent;

namespace FolderComparer.Pipes
{
    public abstract class Pipe
    {
        internal abstract void Execute();
    }

    public class Pipe<T1, T2> : ContinuablePipe<T2>
    {
        private readonly ContinuablePipe<T1> _prevPipe;

        internal Pipe(IPipeMiddleItem<T1, T2> pipeAction, ContinuablePipe<T1> prevPipe) : base(pipeAction)
        {
            _prevPipe = prevPipe;
        }

        public FinishPipe<T2, TResult> FinishWith<TResult>(Func<BlockingCollection<T2>, TResult> func)
        {
            return new FinishPipe<T2, TResult>(func, PipeItem.Output, this);
        }

        internal override void Execute()
        {
            _prevPipe.Execute();
            PipeItem.Execute();
        }
    }
}
