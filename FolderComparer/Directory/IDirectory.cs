using System.Collections.Generic;

namespace FolderComparer
{
    public interface IDirectory
    {
        string Path { get; }
        bool IsEmpty { get; }
    }
    public interface IDirectory<out T> : IDirectory
        where T : IFile
    {
        IEnumerable<T> Enumerate();
    }
}