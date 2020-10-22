using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public interface IHashedFileBlock
    {
        int BlockNumber { get; }
        Guid FileId { get; }
        bool IsLastBlock { get; }
    }

    public interface IFileBlock : IHashedFileBlock
    {
        IHashedFileBlock HashBlock(HashAlgorithm algorithm);
    }
}
