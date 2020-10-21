using System;

namespace FolderComparer.Pipes
{
    public abstract class ContinuablePipe<TOutput> : Pipe
    {
        public IPipeStartItem<TOutput> PipeItem { get; }

        protected ContinuablePipe(IPipeStartItem<TOutput> item)
        {
            PipeItem = item;
        }

        public Pipe<TOutput, TNext> ContinueWith<TNext>(
            Func<IAddingCompletableCollection<TOutput>, IAddingCompletableCollection<TNext>, IPipeMiddleItem<TOutput, TNext>> creator)
        {
            var pipeItem = creator.Invoke(PipeItem.Output, new SimplePushing<TNext>());

            return new Pipe<TOutput, TNext>(pipeItem, this);
        }

        public Pipe<TOutput, TNext> ContinueWith<TNext>(
            Action<IAddingCompletableCollection<TOutput>, IAddingCompletableCollection<TNext>> action)
        {
            var pipeAction = new PipeAction<TOutput, TNext>(action, PipeItem.Output, new SimplePushing<TNext>());

            return new Pipe<TOutput, TNext>(pipeAction, this);
        }
    }
}
