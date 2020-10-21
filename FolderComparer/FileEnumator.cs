using FolderComparer.Pipes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class FileEnumator //: IPipeStartItem<ILocalFile>
    {
        private readonly LocalDirectory[] _directories;

        public FileEnumator(params LocalDirectory[] directories)
        {
            _directories = directories;
        }

        public IProducerConsumerCollection<ILocalFile> Output { get; set; }

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
