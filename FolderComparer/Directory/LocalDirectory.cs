using FolderComparer.Files;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FolderComparer
{
    public class LocalDirectory : IDirectory<LocalFile>, IEnumerable<LocalFile>
    {
        private readonly DirectoryInfo _directoryInfo;
        public LocalDirectory(DirectoryInfo directoryInfo)
        {
            _directoryInfo = directoryInfo;
        }
        public string Path => _directoryInfo.FullName;

        public IEnumerable<LocalFile> Enumerate()
        {
            Guid currentFolderId = Guid.NewGuid();
            foreach (var file in _directoryInfo.EnumerateFiles())
                yield return new LocalFile(file.FullName, currentFolderId);

            foreach (var subDirectory in _directoryInfo.EnumerateDirectories("*", SearchOption.AllDirectories))
            {
                currentFolderId = Guid.NewGuid();
                foreach (var file in subDirectory.EnumerateFiles())
                {
                    yield return new LocalFile(file.FullName, currentFolderId);
                }
            }
        }

        public IEnumerator<LocalFile> GetEnumerator()
        {
            return Enumerate().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
