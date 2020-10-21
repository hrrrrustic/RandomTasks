using System;

namespace FolderComparer.Pipes
{
    public static class Pipeline
    {
        public static StartPipe<TOut> Start<TOut>(IPipeStartItem<TOut> pipeItem)
        {
            return new StartPipe<TOut>(pipeItem);
        }

        public static StartPipe<TOut> Start<TOut>(Func<IAddingCompletableCollection<TOut>, IPipeStartItem<TOut>> creator)
        {
            var pipeAction = creator.Invoke(new SimplePushing<TOut>());

            return Start(pipeAction);
        }
    }
}