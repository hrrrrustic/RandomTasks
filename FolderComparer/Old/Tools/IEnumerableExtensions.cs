using System.Collections.Generic;
using System.Linq;
using FolderComparer.Old.Blocks;
using FolderComparer.Old;
using System;
using System.Security.Cryptography;
using FolderComparer.Folders;

namespace FolderComparer.Tools
{
    public static class IEnumerableExtensions
    {
        public static HashedLocalFolder ToHashedFolder(this Dictionary<DictionaryKeyArray, List<Old.HashedLocalFile>> files, HashAlgorithm algorithm) 
            => FileBlocksHandler.MergeFilesHash(files, algorithm);
        public static Old.HashedLocalFile ToHashedFile(this IEnumerable<HashedFileBlock> blocks, HashAlgorithm algorithm) 
            => FileBlocksHandler.MergeBlocksHash(blocks.ToList(), algorithm);
    }
}