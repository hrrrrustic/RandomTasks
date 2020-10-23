using System;
using System.Security.Cryptography;

namespace FolderComparer
{
    public interface IFileBlock
    {
        int BlockNumber { get; }
        Guid FileId { get; }
        bool IsLastBlock { get; }
    }
    public interface IHashableFileBlock : IFileBlock
    {
        IHashedFileBlock HashBlock(HashAlgorithm algorithm);
    }
    public interface IHashedFileBlock : IFileBlock
    {
        byte[] Hash { get; }
        byte[] MergeHash(byte[] anotherBlock);
    }
}