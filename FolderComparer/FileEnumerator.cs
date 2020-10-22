using FolderComparer.Tools;
using Pipelines.Pipes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
