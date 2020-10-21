namespace Pipelines.Pipes
{
    public class StartPipe<TOut> : ContinuablePipe<TOut>
    {
        public StartPipe(IPipeStartItem<TOut> action) : base(action, false) {}

        internal override void Execute()
        {
            PipeItem.Execute();
        }
    }
}