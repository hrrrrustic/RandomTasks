using System;
using System.Collections.Concurrent;

namespace FolderComparer.Pipes
{
    internal class PipeAction<TIn, TOut> : IPipeMiddleItem<TIn, TOut>
    {
        private readonly Action<BlockingCollection<TIn>, BlockingCollection<TOut>> _action;
        public PipeAction(Action<BlockingCollection<TIn>, BlockingCollection<TOut>> action,
            BlockingCollection<TIn> input, BlockingCollection<TOut> output)
        {
            _action = action;
            Input = input;
            Output = output;
        }

        public BlockingCollection<TIn> Input { get; }

        public BlockingCollection<TOut> Output { get; }

        public void Execute()
        {
            _action.Invoke(Input, Output);
        }
    }
}
