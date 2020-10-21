namespace FolderComparer.Pipes
{
    public class StartPipe<TOut> : ContinuablePipe<TOut>
    {
        public StartPipe(IPipeStartItem<TOut> action) : base(action) {}

        internal override void Execute()
        {
            PipeItem.Execute();
        }
    }
}