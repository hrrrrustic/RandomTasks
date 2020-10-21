using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Pipelines.Pipes
{
    public class FinishPipe<TIn, TResult> : Pipe
    {
        private readonly Func<BlockingCollection<TIn>, TResult> _func;
        private readonly ContinuablePipe<TIn> _prevPipe;

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
            if (IsParallel)
            {
                Thread runThread = new Thread(_prevPipe.Execute);
                runThread.Start();
                var res = _func.Invoke(Input);
                runThread.Join();
                return res;
            }

            _prevPipe.Execute();
            var res2 = _func.Invoke(Input);
            return res2;
        }

        internal override void Execute()
        {
            GetResult();
        }
    }
}
