using System;

namespace FolderComparer
{
    public interface IFile
    {
    }

    public interface ILocalFile : IFile
    { 
        String Path { get; }
    }
}
