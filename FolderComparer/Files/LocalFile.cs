using System;

namespace FolderComparer.Files
{
    public class LocalFile
    {
        public readonly FileInfo FileInfo;

        public LocalFile(String filePath, Guid folderId)
        {
            FileInfo = new FileInfo(filePath, folderId, Guid.NewGuid());
        }
    }
}