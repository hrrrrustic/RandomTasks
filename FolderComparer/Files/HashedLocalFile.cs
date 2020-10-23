using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer.Files
{
    public class HashedLocalFile : IHashedFile
    {
        public byte[] Hash { get; }

        public Guid FolderId { get; }

        public HashedLocalFile(byte[] hash, Guid folderId)
        {
            Hash = hash;
            FolderId = folderId;
        }
    }
}
