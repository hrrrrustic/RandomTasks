using FolderComparer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FolderComparer.Folders
{
    public class HashedLocalFolder
    {
        public readonly IReadOnlyDictionary<DictionaryKeyArray, List<HashedLocalFile>> HashedFiles;
        public readonly Byte[] Hash;

        public HashedLocalFolder(Dictionary<DictionaryKeyArray, List<HashedLocalFile>> hashedFiles, Byte[] hash) => (HashedFiles, Hash) = (hashedFiles, hash);

        #region ObjectOverride
        public Boolean Equals(HashedLocalFolder other)
        {
            if (other is null)
                return false;

            return Hash.SequenceEqual(other.Hash);
        }

        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is HashedLocalFolder hashedFolder && Equals(hashedFolder);
        }

        public override Int32 GetHashCode()
        {
            return HashCode.Combine(HashedFiles, Hash);
        }

        #endregion

        public static Boolean operator ==(HashedLocalFolder first, HashedLocalFolder second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (first is null || second is null)
                return false;

            return first.Hash.SequenceEqual(second.Hash);
        }

        public static Boolean operator !=(HashedLocalFolder first, HashedLocalFolder second)
        {
            return !(first == second);
        }

        public DirectoryCompareResult CompareTo(HashedLocalFolder other)
        {
            if (Hash == other.Hash)
                return DirectoryCompareResult.IdenticalFoldersResult;

            List<(String, String)> matches = new();
            List<String> differences = new();

            var keys = HashedFiles
                .Keys
                .Union(other.HashedFiles.Keys)
                .Distinct();

            foreach (var key in keys)
            {
                if (!HashedFiles.ContainsKey(key))
                {
                    var diff = GetFileNames(other.HashedFiles[key]);
                    differences.AddRange(diff);
                    continue;
                }

                if (!other.HashedFiles.ContainsKey(key))
                {
                    var diff = GetFileNames(HashedFiles[key]);
                    differences.AddRange(diff);
                    continue;
                }

                var currentFiles = HashedFiles[key];
                var otherFiles = other.HashedFiles[key];

                if (currentFiles.Count == otherFiles.Count)
                {
                    matches.AddRange(currentFiles.Select((k, i) => (k.LocalFile.FilePath, otherFiles[i].LocalFile.FilePath)));
                    continue;
                }

                Int32 minCount = Math.Min(currentFiles.Count, otherFiles.Count);

                for (int i = 0; i < minCount; i++)
                    matches.Add((currentFiles[i].LocalFile.FilePath, otherFiles[i].LocalFile.FilePath));

                WriteDiffIfExtraFiles(currentFiles, minCount);
                WriteDiffIfExtraFiles(otherFiles, minCount);
            }

            return new DirectoryCompareResult(matches, differences);

            IEnumerable<String> GetFileNames(IEnumerable<HashedLocalFile> files) => files.Select(k => k.LocalFile.FilePath);

            void WriteDiffIfExtraFiles(List<HashedLocalFile> files, Int32 count)
            {
                if(files.Count > count)
                    differences.AddRange(GetFileNames(files.Skip(count)));
            }
        }
    }
}