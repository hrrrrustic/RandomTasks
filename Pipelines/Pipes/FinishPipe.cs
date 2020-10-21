using System;
using System.Collections.Concurrent;

namespace Pipelines.Pipes
{
    public class FinishPipe<TIn, TResult> : Pipe
    {
        private readonly Func<BlockingCollection<TIn>, TResult> _func;
        private readonly Pipe _prevPipe;

        public BlockingCollection<TIn> Input { get; set; }

        internal FinishPipe(Func<BlockingCollection<TIn>, TResult> func,
            BlockingCollection<TIn> input,
            ContinuablePipe<TIn> prevPipe,
            bool isParallel) : base(isParallel)
        {
            _func = func;
            Input = input;
            _prevPipe = prevPipe;
        }
        public TResult GetResult()
        {
            _prevPipe.Execute();
            return _func.Invoke(Input);
        }

        internal override void Execute()
        {
            GetResult();
        }
    }
}
