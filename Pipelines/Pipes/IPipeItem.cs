using System.Collections.Concurrent;

namespace Pipelines.Pipes
{
    public interface IPipeItem
    {
        void Execute();
    }
    public interface IPipeStartItem<TOut> : IPipeItem
    {
        BlockingCollection<TOut> Output { get; }
    }
    public interface IPipeLastItem<TIn> : IPipeItem
    {
        BlockingCollection<TIn> Input { get; }
    }
    public interface IPipeMiddleItem<TIn, TOut> : IPipeLastItem<TIn>, IPipeStartItem<TOut>
    {
    }
}
