using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    interface IFileBlockReader<T> : IProducerConsumerCollection<T>
        where T : IFile
    {
        void StartReading();
    }
}
