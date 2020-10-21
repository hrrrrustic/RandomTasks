using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public interface IDirectory<out T> where T : IFile
    {
        string Path { get; }
        bool IsEmpty { get; }
        IEnumerable<T> Enumerate();
    }
}