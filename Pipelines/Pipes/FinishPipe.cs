using System;

namespace FolderComparer.Pipes
{
    public class FinishPipe<TIn, TResult> : IPipeLastItem<TIn>
    {
        private readonly Func<IAddingCompletableCollection<TIn>, TResult> _func;
        private readonly Pipe _prevPipe;

        public IAddingCompletableCollection<TIn> Input { get; set; }

        public FinishPipe(Func<IAddingCompletableCollection<TIn>, TResult> func,
            IAddingCompletableCollection<TIn> input,
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
