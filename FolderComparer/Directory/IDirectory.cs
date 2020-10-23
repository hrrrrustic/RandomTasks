using System.Collections.Generic;

namespace FolderComparer
{
    public interface IDirectory<out T> where T : IFile
    {
        string Path { get; }
        bool IsEmpty { get; }
        IEnumerable<T> Enumerate();
    }
}