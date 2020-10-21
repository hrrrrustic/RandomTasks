using System;
using System.Collections.Concurrent;

namespace FolderComparer.Pipes
{
    public class FinishPipe<TIn, TResult> : IPipeLastItem<TIn>
    {
        private readonly Func<BlockingCollection<TIn>, TResult> _func;
        private readonly Pipe _prevPipe;

        public BlockingCollection<TIn> Input { get; set; }

        public FinishPipe(Func<BlockingCollection<TIn>, TResult> func,
            BlockingCollection<TIn> input,
            ContinuablePipe<TIn> prevPipe)
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

        public void Execute()
        {
            GetResult();
        }
    }
}
