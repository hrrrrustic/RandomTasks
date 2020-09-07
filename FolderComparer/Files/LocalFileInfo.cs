using System;

namespace FolderComparer.Files
{
    public class LocalFileInfo
    {
        public readonly String FilePath;
        public readonly Guid FolderId;
        public readonly Guid FileId;

        public LocalFileInfo(String filePath, Guid folderId, Guid fileId)
        {
            FilePath = filePath;
            FolderId = folderId;
            FileId = fileId;
        }
    }
}