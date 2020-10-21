namespace FolderComparer.Pipes
{
    public interface IPipeItem
    {
        void Execute();
    }
    public interface IPipeStartItem<TOut> : IPipeItem
    {
        IAddingCompletableCollection<TOut> Output { get; }
    }
    public interface IPipeLastItem<TIn> : IPipeItem
    {
        IAddingCompletableCollection<TIn> Input { get; }
    }
    public interface IPipeMiddleItem<TIn, TOut> : IPipeLastItem<TIn>, IPipeStartItem<TOut>
    {
    }
}
