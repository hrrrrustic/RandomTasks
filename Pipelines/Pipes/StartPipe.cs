﻿namespace FolderComparer.Pipes
{
    public class StartPipe<T1> : ContinuablePipe<T1>
    {
        public StartPipe(IPipeStartItem<T1> action) : base(action) {}

        internal override void Execute()
        {
            PipeItem.Execute();
        }
    }
}