using System;
using System.Linq;

namespace FolderComparer.Files
{
    public class HashedLocalFile : IHashedFile
    {
        public byte[] Hash { get; }
        public Guid DirectoryId { get; }
        public Guid FileId { get; }
        public string Path { get; }

        public HashedLocalFile(byte[] hash, FileInfo fileInfo)
        {
            Hash = hash;
            DirectoryId = fileInfo.FolderId;
            FileId = fileInfo.FileId;
            Path = fileInfo.FilePath;
        }

        public byte[] MergeHash(byte[] anotherFile)
        {
            if (Hash.Length != anotherFile.Length)
                throw new Exception();

            return Hash.Select((k, i) => (Byte)(k ^ anotherFile[i])).ToArray();
        }

        public bool Equals(IHashedFile? other)
        {
            if (other is null)
                return false;

            return Hash.SequenceEqual(other.Hash);
        }
    }
}
