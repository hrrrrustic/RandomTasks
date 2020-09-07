using System;

namespace FolderComparer
{
    public class FileInfo
    {
        public readonly String FilePath;
        public readonly Guid FolderId;
        public readonly Guid FileId;
        public readonly Int32 BlockCount;

        public FileInfo(String filePath, Guid folderId, Guid fileId, Int32 blockCount)
        {
            FilePath = filePath;
            FolderId = folderId;
            FileId = fileId;
            BlockCount = blockCount;
        }
    }
}