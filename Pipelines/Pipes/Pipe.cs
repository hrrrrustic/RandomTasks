using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Pipelines.Pipes
{
    public abstract class Pipe : IDisposable
    {
        protected bool IsParallel { get; }
        protected Pipe(bool isParallel)
        {
            IsParallel = isParallel;
        }
        internal abstract void Execute();

        public abstract void Dispose();
    }

    public class Pipe<TIn, TOut> : ContinuablePipe<TOut>
    {
        private readonly ContinuablePipe<TIn> _prevPipe;
        private bool _isPrevPipeDisposed;
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
            if (IsParallel)
                ExecuteParallel();
            else
                ExecuteSequentially();

            Dispose();
        }

        private void ExecuteParallel()
        {
            Thread runThread = new Thread(_prevPipe.Execute);
            runThread.Start();
            PipeItem.Execute();
            runThread.Join();
        }

        private void ExecuteSequentially()
        {
            _prevPipe.Execute();
            PipeItem.Execute();
        }

        public override void Dispose()
        {
            if (_isPrevPipeDisposed)
            {
                PipeItem.Dispose();
                return;
            }

            _prevPipe.Dispose();
            _isPrevPipeDisposed = true;
        }
    }
}