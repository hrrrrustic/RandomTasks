using System;

namespace FolderComparer.Files
{
    public class LocalFile : IFile
    {
        public string Path { get; }
        public Guid FileId { get; }
        public Guid DirectoryId { get; }

        public LocalFile(string path, Guid folderId)
        {
            DirectoryId = folderId;
            FileId = Guid.NewGuid();
            Path = path;
        }
    }
}
