using System;

namespace FolderComparer
{
    public interface IFile
    {
        Guid DirectoryId { get; }
        Guid FileId { get; }
        String Path { get; }
    }
}