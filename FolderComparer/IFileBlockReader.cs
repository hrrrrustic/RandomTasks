using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    interface IFileBlockReader<T> : IAddingCompletableCollection<T>
        where T : IFile
    {
        void StartReading();
    }
}
