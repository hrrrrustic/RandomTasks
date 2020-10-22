using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Pipelines.Pipes
{
    public class FinishPipe<TIn, TResult> : Pipe
    {
        private readonly Func<BlockingCollection<TIn>, TResult> _func;
        private readonly ContinuablePipe<TIn> _prevPipe;

        private BlockingCollection<TIn> Input { get; }

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
            TResult result;
            if (IsParallel)
                result = GetResultParallel();
            else
                result = GetResultSequentially();

            Dispose();

            return result;
        }

        private TResult GetResultSequentially()
        {
            _prevPipe.Execute();
            return _func.Invoke(Input);
        }
        private TResult GetResultParallel()
        {
            Thread runThread = new Thread(_prevPipe.Execute);
            runThread.Start();
            var res = _func.Invoke(Input);
            runThread.Join();
            return res;
        }
        internal override void Execute()
        {
            GetResult();
        }

        public override void Dispose()
        {
            _prevPipe.Dispose();
            Input.Dispose();
        }
    }
}
