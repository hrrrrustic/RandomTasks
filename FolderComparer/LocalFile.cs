using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FolderComparer
{
    public class LocalFile
    {
        public readonly String FilePath;
        private Byte[] FileHash { get; set; }

        public LocalFile(String filePath)
        {
            FilePath = filePath;
        }
    }
}