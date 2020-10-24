using FolderComparer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FolderComparer.Directory
{
    public class HashedLocalDirectory : IHashedDirectory
    {
        public byte[] Hash { get; }
        public IReadOnlyCollection<IHashedFile> HashedFiles { get; }

        public HashedLocalDirectory(byte[] hash, IReadOnlyCollection<IHashedFile> hashedFiles)
        {
            HashedFiles = hashedFiles;
            Hash = hash;
        }

        public DirectoryCompareResult Compare(IHashedDirectory another)
        {
            List<(String, String)> matches = new List<(string, string)>();
            List<String> differences = new List<string>();

            var currentFiles = HashedFiles.ToList();

            foreach (var item in another.HashedFiles)
            {
                var index = currentFiles.IndexOf(item);
                if(index != -1)
                {
                    matches.Add((item.Path, currentFiles[index].Path));
                    currentFiles.RemoveAt(index);
                }
                else
                {
                    differences.Add(item.Path);
                }
            }

            foreach (var item in currentFiles)
            {
                differences.Add(item.Path);
            }

            return new DirectoryCompareResult(matches, differences);
        }
    }
}
