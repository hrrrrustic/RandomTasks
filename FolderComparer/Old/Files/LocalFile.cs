using System;
using System.IO;

namespace FolderComparer.Old.Files
{
    public class LocalFile2
    {
        public readonly LocalFileInfo FileInfo;
        public Boolean IsExist => File.Exists(FileInfo.FilePath);
        public LocalFile2(String filePath, Guid folderId)
        {
            FileInfo = new(filePath, folderId, Guid.NewGuid());
        }
    }
}