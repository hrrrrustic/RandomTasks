using System.Collections.Generic;
using System.Linq;
using FolderComparer.Files;
using FolderComparer.Folders;

namespace FolderComparer.Tools
{
    public static class IEnumerableExtensions
    {
        public static HashedLocalFolder ToHashedFolder(this IEnumerable<HashedLocalFile> files) => FileBlocksHandler.MergeFilesHash(files.ToList());
    }
}