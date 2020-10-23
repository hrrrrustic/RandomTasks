using Pipelines.Pipes;
using System.Collections.Concurrent;

namespace FolderComparer
{
    public sealed class FileEnumerator : IPipeStartItem<ILocalFile>
    {
        private readonly LocalDirectory[] _directories;
        public BlockingCollection<ILocalFile> Output { get; set; }

        public FileEnumerator(BlockingCollection<ILocalFile> output, params LocalDirectory[] directories)
        {
            Output = output;
            _directories = directories;
        }

        public void Dispose()
        {
            Output.Dispose();
        }

        public void Enumerate()
        {
            foreach (var directory in _directories)
            {
                foreach (var file in directory)
                {
                    Output.Add(file);
                }
            }
        }

        public void Execute()
        {
            Enumerate();
            Output.CompleteAdding();
        }
    }
}
