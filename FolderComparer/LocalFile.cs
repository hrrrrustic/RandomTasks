using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class LocalFile
    {
        public readonly String FilePath;

        public readonly Guid FileId;

        public readonly Guid FolderId;

        public LocalFile(String filePath, Guid folderId)
        {
            FilePath = filePath;
            FileId = Guid.NewGuid();
            FolderId = folderId;
        }
    }
}