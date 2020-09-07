using System;

namespace FolderComparer.Files
{
    public class LocalFile
    {
        public readonly LocalFileInfo FileInfo;

        public LocalFile(String filePath, Guid folderId)
        {
            FileInfo = new(filePath, folderId, Guid.NewGuid());
        }
    }
}