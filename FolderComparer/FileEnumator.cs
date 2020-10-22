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
    public class FileEnumator : IPipeStartItem<ILocalFile>
    {
        private readonly LocalDirectory[] _directories;

        public FileEnumator(BlockingCollection<ILocalFile> output, params LocalDirectory[] directories)
        {
            Output = output;
            _directories = directories;
        }

        public BlockingCollection<ILocalFile> Output { get; set; }

        public void Dispose()
        {
            Output.Dispose();
        }

        public void Enumerate()
        {
            throw new Exception();
        }

        public void Execute()
        {
            Enumerate();
        }
    }
}
