using System;

namespace FolderComparer.Files
{
    public class FileInfo
    {
        public readonly String FilePath;
        public readonly Guid FolderId;
        public readonly Guid FileId;

        public FileInfo(String filePath, Guid folderId, Guid fileId)
        {
            FilePath = filePath;
            FolderId = folderId;
            FileId = fileId;
        }
    }
}