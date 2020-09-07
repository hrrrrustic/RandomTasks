using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FolderComparer
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