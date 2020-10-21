using System.Collections.Generic;
using System.Linq;
using FolderComparer.Blocks;
using FolderComparer.Folders;
using System;
using System.Security.Cryptography;

namespace FolderComparer.Tools
{
    public static class IEnumerableExtensions
    {
        public static HashedLocalFolder ToHashedFolder(this Dictionary<DictionaryKeyArray, List<HashedLocalFile>> files, HashAlgorithm algorithm) 
            => FileBlocksHandler.MergeFilesHash(files, algorithm);
        public static HashedLocalFile ToHashedFile(this IEnumerable<HashedFileBlock> blocks, HashAlgorithm algorithm) 
            => FileBlocksHandler.MergeBlocksHash(blocks.ToList(), algorithm);
    }
}