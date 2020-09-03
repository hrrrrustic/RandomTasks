using System;
using System.IO;
using System.Security.Cryptography;

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

        public Byte[] GetHashCode(HashAlgorithm hashAlgorithm) => 
            FileHash ??= hashAlgorithm.ComputeHash(File.ReadAllBytes(FilePath));

        public void ComputeHashCode(HashAlgorithm hashAlgorithm) => 
            FileHash = hashAlgorithm.ComputeHash(File.ReadAllBytes(FilePath));
    }
}