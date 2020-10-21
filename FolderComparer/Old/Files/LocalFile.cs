using System;
using System.IO;

namespace FolderComparer.Files
{
    public class LocalFile
    {
        public readonly LocalFileInfo FileInfo;
        public Boolean IsExist => File.Exists(FileInfo.FilePath);
        public LocalFile(String filePath, Guid folderId)
        {
            FileInfo = new(filePath, folderId, Guid.NewGuid());
        }
    }
}