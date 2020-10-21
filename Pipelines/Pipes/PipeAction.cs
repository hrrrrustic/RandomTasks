using System;

namespace FolderComparer.Pipes
{
    internal class PipeAction<TIn, TOut> : IPipeMiddleItem<TIn, TOut>
    {
        private readonly Action<IAddingCompletableCollection<TIn>, IAddingCompletableCollection<TOut>> _action;
        public PipeAction(Action<IAddingCompletableCollection<TIn>, IAddingCompletableCollection<TOut>> action,
            IAddingCompletableCollection<TIn> input, IAddingCompletableCollection<TOut> output)
        {
            _action = action;
            Input = input;
            Output = output;
        }

        public IAddingCompletableCollection<TIn> Input { get; }

        public IAddingCompletableCollection<TOut> Output { get; }

        public void Execute()
        {
            _action.Invoke(Input, Output);
        }
    }
}
